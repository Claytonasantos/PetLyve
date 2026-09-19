namespace PetLyve.Application.DTOs.Servico;

public class ServicoResponseDto
{
    public Guid ServicoId { get; set; }

    public string NomeServico { get; set; }

    public decimal Preco { get; set; }
}