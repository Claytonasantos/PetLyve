using System.ComponentModel.DataAnnotations;

namespace PetLyve.Application.DTOs.Animal;

public class AnimalRequestDto
{
    [Required]
    public string Nome { get; set; }

    [Required]
    public string Especie { get; set; }

    [Required]
    public Guid DonoId { get; set; }
}