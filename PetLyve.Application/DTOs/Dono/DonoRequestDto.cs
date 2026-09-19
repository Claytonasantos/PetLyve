using System.ComponentModel.DataAnnotations;

namespace PetLyve.Application.DTOs.Dono;

public class DonoRequestDto
{
    [Required]
    public string Nome { get; set; }

    [Required]
    public string Telefone { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}