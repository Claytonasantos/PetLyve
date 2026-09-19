namespace PetLyve.Application.DTOs.Animal;

public class AnimalResponseDto
{
    public Guid AnimalId { get; set; }

    public string Nome { get; set; }

    public string Especie { get; set; }

    public Guid DonoId { get; set; }
}