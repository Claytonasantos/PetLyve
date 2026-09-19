using Microsoft.AspNetCore.Mvc;
using PetLyve.Application;
using PetLyve.Application.DTOs.Servico;
using PetLyve.Domain.Entities;

namespace PetLyve.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicosController : ControllerBase
{
    private readonly IRepository<Servico> _repository;

    public ServicosController(IRepository<Servico> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Lista todos os serviços cadastrados.
    /// </summary>
    /// <returns>Lista de serviços.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServicoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServicoResponseDto>>> GetAll()
    {
        var servicos = await _repository.GetAllAsync();

        var response = servicos.Select(servico => new ServicoResponseDto
        {
            ServicoId = servico.ServicoId,
            NomeServico = servico.NomeServico,
            Preco = servico.Preco
        });

        return Ok(response);
    }

    /// <summary>
    /// Busca um serviço pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do serviço.</param>
    /// <returns>Dados do serviço encontrado.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ServicoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServicoResponseDto>> GetById(Guid id)
    {
        var servico = await _repository.GetByIdAsync(id);

        if (servico is null)
        {
            return NotFound();
        }

        var response = new ServicoResponseDto
        {
            ServicoId = servico.ServicoId,
            NomeServico = servico.NomeServico,
            Preco = servico.Preco
        };

        return Ok(response);
    }

    /// <summary>
    /// Cadastra um novo serviço.
    /// </summary>
    /// <param name="request">Dados do serviço.</param>
    /// <returns>Dados do serviço criado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ServicoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServicoResponseDto>> Create(
        [FromBody] ServicoRequestDto request)
    {
        var servico = new Servico
        {
            ServicoId = Guid.NewGuid(),
            NomeServico = request.NomeServico,
            Preco = request.Preco
        };

        await _repository.AddAsync(servico);

        var response = new ServicoResponseDto
        {
            ServicoId = servico.ServicoId,
            NomeServico = servico.NomeServico,
            Preco = servico.Preco
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = servico.ServicoId },
            response);
    }
}