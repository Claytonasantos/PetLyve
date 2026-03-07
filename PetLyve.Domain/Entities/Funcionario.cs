namespace PetLyve.Domain.Entities;

public class Funcionario
{
    public Guid FuncionarioId { get; set; }
    public string Nome { get; set; }
    public string Cargo { get; set; }

    public List<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}