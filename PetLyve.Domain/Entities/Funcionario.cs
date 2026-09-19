using PetLyve.Domain.Entities.Base;

namespace PetLyve.Domain.Entities;

public class Funcionario : BaseEntity
{
    public Guid FuncionarioId { get; set; }

    public override Guid Id => FuncionarioId;

    public string Nome { get; set; }
    public string Cargo { get; set; }

    public List<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}