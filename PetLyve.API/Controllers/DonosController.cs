using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PetLyve.API.Configuration.RateLimiting;
using PetLyve.Application.DTOs;
using PetLyve.Application.DTOs.Dono;
using PetLyve.Application.Pagination;
using PetLyve.Application.Services;

namespace PetLyve.API.Controllers;

/// <summary>
/// Recurso versionado do CP5. A v1 (deprecada) mantém o contrato do CP3
/// (lista completa); a v2 devolve um envelope paginado. As duas versões
/// usam o mesmo <see cref="DonoService"/>.
/// </summary>
[ApiController]
[ApiVersion("1.0", Deprecated = true)]
[ApiVersion("2.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DonosController : ControllerBase
{
    private readonly DonoService _service;
    private readonly ILogger<DonosController> _logger;

    public DonosController(
        DonoService service,
        ILogger<DonosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// [v1 - DEPRECIADA] Lista todos os donos cadastrados, sem paginação.
    /// </summary>
    /// <remarks>
    /// Contrato antigo (CP3): devolve um array com todos os donos.
    /// Mantido apenas para compatibilidade; use a v2.
    /// </remarks>
    /// <returns>Lista de donos.</returns>
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(IEnumerable<DonoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DonoResponseDto>>> GetAll()
    {
        var donos = await _service.GetAllAsync();

        return Ok(donos);
    }

    /// <summary>
    /// [v2] Lista os donos de forma paginada, ordenados por nome.
    /// </summary>
    /// <param name="page">Página desejada (padrão 1, mínimo 1).</param>
    /// <param name="pageSize">Itens por página (padrão 20, entre 1 e 100).</param>
    /// <returns>Envelope com a página e os totais.</returns>
    /// <response code="200">Página solicitada. Além do total, items vem vazio.</response>
    /// <response code="400">page ou pageSize fora do intervalo permitido.</response>
    /// <response code="429">Limite de requisições da listagem excedido.</response>
    [HttpGet]
    [MapToApiVersion("2.0")]
    [EnableRateLimiting(RateLimitPolicies.Leitura)]
    [ProducesResponseType(typeof(PagedResponseDto<DonoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests, "application/problem+json")]
    public async Task<ActionResult<PagedResponseDto<DonoResponseDto>>> GetPaged(
        [FromQuery] int page = PageRequest.DefaultPage,
        [FromQuery] int pageSize = PageRequest.DefaultPageSize)
    {
        var donos = await _service.GetPagedAsync(page, pageSize);

        return Ok(donos);
    }

    /// <summary>
    /// Busca um dono pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do dono.</param>
    /// <returns>Dados do dono encontrado.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DonoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DonoResponseDto>> GetById(Guid id)
    {
        var dono = await _service.GetByIdAsync(id);

        return Ok(dono);
    }

    /// <summary>
    /// Cadastra um novo dono.
    /// </summary>
    /// <param name="request">Dados do dono.</param>
    /// <returns>Dados do dono criado.</returns>
    /// <response code="429">Limite de cadastros por janela excedido (veja Retry-After).</response>
    [HttpPost]
    [EnableRateLimiting(RateLimitPolicies.Escrita)]
    [ProducesResponseType(typeof(DonoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests, "application/problem+json")]
    public async Task<ActionResult<DonoResponseDto>> Create(
        [FromBody] DonoRequestDto request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation(
            "Iniciando cadastro de dono. Nome={Nome}, TraceId={TraceId}",
            request.Nome,
            traceId);

        try
        {
            var response = await _service.CreateAsync(request);

            _logger.LogInformation(
                "Dono cadastrado com sucesso. DonoId={DonoId}, TraceId={TraceId}",
                response.DonoId,
                traceId);

            // version = null descarta o segmento de versão da rota atual:
            // o Location fica /api/Donos/{id}, válido na v1 e na v2.
            return CreatedAtAction(
                nameof(GetById),
                new { id = response.DonoId, version = (string?)null },
                response);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Erro ao cadastrar dono. Nome={Nome}, TraceId={TraceId}",
                request.Nome,
                traceId);

            throw;
        }
    }
}