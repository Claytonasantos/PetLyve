using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PetLyve.Application.Pagination;

namespace PetLyve.API.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Ocorreu uma exceção não tratada. TraceId={TraceId}",
            traceId);

        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            InvalidOperationException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var title = statusCode switch
        {
            StatusCodes.Status400BadRequest => "Requisição inválida",
            StatusCodes.Status404NotFound => "Recurso não encontrado",
            StatusCodes.Status409Conflict => "Conflito",
            _ => "Erro interno do servidor"
        };

        var detail = statusCode switch
        {
            StatusCodes.Status400BadRequest =>
                "Os dados enviados são inválidos.",

            StatusCodes.Status404NotFound =>
                "O recurso solicitado não foi encontrado.",

            StatusCodes.Status409Conflict =>
                "A operação não pôde ser concluída devido a um conflito.",

            _ =>
                "Ocorreu um erro interno no servidor."
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        // Paginação inválida é erro do cliente com regra conhecida:
        // a mensagem é segura e diz exatamente o que falhou.
        if (exception is InvalidPaginationException paginationException)
        {
            problemDetails.Title = "Parâmetros de paginação inválidos";
            problemDetails.Detail = paginationException.Message;
            problemDetails.Extensions["errors"] = paginationException.Errors;
        }

        problemDetails.Extensions["traceId"] = traceId;

        httpContext.Response.StatusCode = statusCode;

        // O content type precisa ir no WriteAsJsonAsync: atribuí-lo antes
        // não adianta, pois a sobrecarga sem ele grava application/json.
        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            options: null,
            contentType: "application/problem+json",
            cancellationToken: cancellationToken);

        return true;
    }
}