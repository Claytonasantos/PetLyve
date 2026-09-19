using PetLyve.Domain.Entities.Base;

namespace PetLyve.Domain.Entities;

public class Dono : BaseEntity
{
    public Guid DonoId { get; set; }

    public override Guid Id => DonoId;

    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }

    public List<Animal> Animais { get; set; } = new List<Animal>();
}