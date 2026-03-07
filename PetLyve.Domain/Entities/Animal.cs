namespace PetLyve.Domain.Entities;

public class Animal
{
    public Guid AnimalId { get; set; }

    public string Nome { get; set; }

    public string Especie { get; set; }

    public Guid DonoId { get; set; }
}