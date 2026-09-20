using Microsoft.AspNetCore.Mvc;
using PetLyve.Application.DTOs.Dono;
using PetLyve.Application.Services;

namespace PetLyve.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    /// Lista todos os donos cadastrados.
    /// </summary>
    /// <returns>Lista de donos.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DonoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DonoResponseDto>>> GetAll()
    {
        var donos = await _service.GetAllAsync();

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
    [HttpPost]
    [ProducesResponseType(typeof(DonoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.DonoId },
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