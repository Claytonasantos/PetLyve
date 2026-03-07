namespace PetLyve.Domain.Entities;

public class Dono
{
    public Guid DonoId { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }

    public List<Animal> Animais { get; set; } = new List<Animal>();
}