using PetLyve.Application.DTOs.Dono;
using PetLyve.Domain.Entities;

namespace PetLyve.Application.Services;

public class DonoService
{
    private readonly IRepository<Dono> _repository;

    public DonoService(IRepository<Dono> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DonoResponseDto>> GetAllAsync()
    {
        var donos = await _repository.GetAllAsync();

        return donos.Select(MapToResponse);
    }

    public async Task<DonoResponseDto> GetByIdAsync(Guid id)
    {
        var dono = await _repository.GetByIdAsync(id);

        if (dono is null)
        {
            throw new KeyNotFoundException(
                $"Dono com ID {id} não encontrado.");
        }

        return MapToResponse(dono);
    }

    public async Task<DonoResponseDto> CreateAsync(DonoRequestDto request)
    {
        var dono = new Dono
        {
            DonoId = Guid.NewGuid(),
            Nome = request.Nome,
            Telefone = request.Telefone,
            Email = request.Email
        };

        await _repository.AddAsync(dono);

        return MapToResponse(dono);
    }

    private static DonoResponseDto MapToResponse(Dono dono)
    {
        return new DonoResponseDto
        {
            DonoId = dono.DonoId,
            Nome = dono.Nome,
            Telefone = dono.Telefone,
            Email = dono.Email
        };
    }
}