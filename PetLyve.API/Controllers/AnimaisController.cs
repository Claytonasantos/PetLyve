using Microsoft.AspNetCore.Mvc;
using PetLyve.Application;
using PetLyve.Application.DTOs.Animal;
using PetLyve.Domain.Entities;

namespace PetLyve.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimaisController : ControllerBase
{
    private readonly IRepository<Animal> _repository;

    public AnimaisController(IRepository<Animal> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Lista todos os animais cadastrados.
    /// </summary>
    /// <returns>Lista de animais.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AnimalResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AnimalResponseDto>>> GetAll()
    {
        var animais = await _repository.GetAllAsync();

        var response = animais.Select(animal => new AnimalResponseDto
        {
            AnimalId = animal.AnimalId,
            Nome = animal.Nome,
            Especie = animal.Especie,
            DonoId = animal.DonoId
        });

        return Ok(response);
    }

    /// <summary>
    /// Busca um animal pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do animal.</param>
    /// <returns>Dados do animal encontrado.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AnimalResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnimalResponseDto>> GetById(Guid id)
    {
        var animal = await _repository.GetByIdAsync(id);

        if (animal is null)
        {
            return NotFound();
        }

        var response = new AnimalResponseDto
        {
            AnimalId = animal.AnimalId,
            Nome = animal.Nome,
            Especie = animal.Especie,
            DonoId = animal.DonoId
        };

        return Ok(response);
    }

    /// <summary>
    /// Cadastra um novo animal.
    /// </summary>
    /// <param name="request">Dados do animal.</param>
    /// <returns>Dados do animal criado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AnimalResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnimalResponseDto>> Create(
        [FromBody] AnimalRequestDto request)
    {
        var animal = new Animal
        {
            AnimalId = Guid.NewGuid(),
            Nome = request.Nome,
            Especie = request.Especie,
            DonoId = request.DonoId
        };

        await _repository.AddAsync(animal);

        var response = new AnimalResponseDto
        {
            AnimalId = animal.AnimalId,
            Nome = animal.Nome,
            Especie = animal.Especie,
            DonoId = animal.DonoId
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = animal.AnimalId },
            response);
    }
}