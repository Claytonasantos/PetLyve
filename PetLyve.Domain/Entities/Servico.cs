using PetLyve.Domain.Entities.Base;

namespace PetLyve.Domain.Entities;

public class Servico : BaseEntity
{
    public Guid ServicoId { get; set; }

    public override Guid Id => ServicoId;

    public string NomeServico { get; set; }
    public decimal Preco { get; set; }

    public List<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();

    public void AlterarPreco(decimal novoPreco)
    {
        if (novoPreco <= 0)
        {
            throw new ArgumentException("O preço deve ser maior que zero.");
        }

        Preco = novoPreco;
    }
}