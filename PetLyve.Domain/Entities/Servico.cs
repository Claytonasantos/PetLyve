using PetLyve.Domain.Entities.Base;

namespace PetLyve.Domain.Entities;

public class Servico : BaseEntity
{
    public Guid ServicoId { get; set; }

    public override Guid Id => ServicoId;

    public string NomeServico { get; set; }
    public decimal Preco { get; set; }

    public List<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}