using PetLyve.Domain.Entities;

namespace PetLyve.Domain.Tests.Entities;

public class ServicoTests
{
    [Fact]
    public void AlterarPreco_ComPrecoValido_DeveAtualizarPreco()
    {
        // Arrange
        var servico = new Servico
        {
            ServicoId = Guid.NewGuid(),
            NomeServico = "Banho",
            Preco = 50
        };

        var novoPreco = 75;

        // Act
        servico.AlterarPreco(novoPreco);

        // Assert
        Assert.Equal(novoPreco, servico.Preco);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(-50.50)]
    public void AlterarPreco_ComPrecoInvalido_DeveLancarArgumentException(decimal precoInvalido)
    {
        // Arrange
        var servico = new Servico
        {
            ServicoId = Guid.NewGuid(),
            NomeServico = "Banho",
            Preco = 50
        };

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => servico.AlterarPreco(precoInvalido));

        // Assert
        Assert.Equal("O preço deve ser maior que zero.", exception.Message);
    }
}