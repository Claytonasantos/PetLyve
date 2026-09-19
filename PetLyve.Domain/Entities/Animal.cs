using PetLyve.Domain.Entities.Base;

namespace PetLyve.Domain.Entities;

public class Animal : BaseEntity
{
    public Guid AnimalId { get; set; }

    public override Guid Id => AnimalId;

    public string Nome { get; set; }
    public string Especie { get; set; }

    public Guid DonoId { get; set; }

    public Dono Dono { get; set; }

    public List<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}