namespace PetLyve.Domain.Entities;

public class Pagamento
{
    public Guid PagamentoId { get; set; }

    public decimal Valor { get; set; }

    public DateTime Data { get; set; }

    public Guid AgendamentoId { get; set; }
}