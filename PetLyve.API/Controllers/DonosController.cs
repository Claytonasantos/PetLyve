using Microsoft.AspNetCore.Mvc;
using PetLyve.Application;
using PetLyve.Application.DTOs.Dono;
using PetLyve.Domain.Entities;

namespace PetLyve.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonosController : ControllerBase
{
    private readonly IRepository<Dono> _repository;

    public DonosController(IRepository<Dono> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Lista todos os donos cadastrados.
    /// </summary>
    /// <returns>Lista de donos.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DonoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DonoResponseDto>>> GetAll()
    {
        var donos = await _repository.GetAllAsync();

        var response = donos.Select(dono => new DonoResponseDto
        {
            DonoId = dono.DonoId,
            Nome = dono.Nome,
            Telefone = dono.Telefone,
            Email = dono.Email
        });

        return Ok(response);
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
        var dono = await _repository.GetByIdAsync(id);

        if (dono is null)
        {
            return NotFound();
        }

        var response = new DonoResponseDto
        {
            DonoId = dono.DonoId,
            Nome = dono.Nome,
            Telefone = dono.Telefone,
            Email = dono.Email
        };

        return Ok(response);
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
        var dono = new Dono
        {
            DonoId = Guid.NewGuid(),
            Nome = request.Nome,
            Telefone = request.Telefone,
            Email = request.Email
        };

        await _repository.AddAsync(dono);

        var response = new DonoResponseDto
        {
            DonoId = dono.DonoId,
            Nome = dono.Nome,
            Telefone = dono.Telefone,
            Email = dono.Email
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = dono.DonoId },
            response);
    }
}