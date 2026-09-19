using PetLyve.Domain.Entities.Base;

namespace PetLyve.Domain.Entities;

public class Pagamento : BaseEntity
{
    public Guid PagamentoId { get; set; }

    public override Guid Id => PagamentoId;

    public decimal Valor { get; set; }
    public DateTime Data { get; set; }

    public Guid AgendamentoId { get; set; }

    public Agendamento Agendamento { get; set; }
}