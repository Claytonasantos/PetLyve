using System.ComponentModel.DataAnnotations;

namespace PetLyve.Application.DTOs.Servico;

public class ServicoRequestDto
{
    [Required]
    public string NomeServico { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Preco { get; set; }
}