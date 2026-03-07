namespace PetLyve.Domain.Entities;

public class Servico
{
    public Guid ServicoId { get; set; }
    public string NomeServico { get; set; }
    public decimal Preco { get; set; }

    public List<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}