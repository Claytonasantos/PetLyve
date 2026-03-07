namespace PetLyve.Domain.Entities;

public class Agendamento
{
    public Guid AgendamentoId { get; set; }
    public DateTime Data { get; set; }

    public Guid AnimalId { get; set; }
    public Guid FuncionarioId { get; set; }
    public Guid ServicoId { get; set; }

    public Animal Animal { get; set; }
    public Funcionario Funcionario { get; set; }
    public Servico Servico { get; set; }

    public Pagamento Pagamento { get; set; }
}