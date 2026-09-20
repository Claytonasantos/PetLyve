using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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

        problemDetails.Extensions["traceId"] = traceId;

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}