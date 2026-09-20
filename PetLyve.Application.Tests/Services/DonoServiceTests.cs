using Moq;
using PetLyve.Application.DTOs.Dono;
using PetLyve.Application.Services;
using PetLyve.Domain.Entities;

namespace PetLyve.Application.Tests.Services;

public class DonoServiceTests
{
    [Fact]
    public async Task CreateAsync_DeveCriarDonoEChamarRepositorio()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Dono>>();

        var request = new DonoRequestDto
        {
            Nome = "Guilherme Sola Garcia",
            Telefone = "11999999999",
            Email = "guilherme@email.com"
        };

        var service = new DonoService(repositoryMock.Object);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.DonoId);
        Assert.Equal(request.Nome, result.Nome);
        Assert.Equal(request.Telefone, result.Telefone);
        Assert.Equal(request.Email, result.Email);

        repositoryMock.Verify(
            repository => repository.AddAsync(
                It.Is<Dono>(dono =>
                    dono.Nome == request.Nome &&
                    dono.Telefone == request.Telefone &&
                    dono.Email == request.Email)),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_QuandoDonoExiste_DeveRetornarDono()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Dono>>();

        var dono = new Dono
        {
            DonoId = Guid.NewGuid(),
            Nome = "Maria Silva",
            Telefone = "11988888888",
            Email = "maria@email.com"
        };

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(dono.DonoId))
            .ReturnsAsync(dono);

        var service = new DonoService(repositoryMock.Object);

        // Act
        var result = await service.GetByIdAsync(dono.DonoId);

        // Assert
        Assert.Equal(dono.DonoId, result.DonoId);
        Assert.Equal(dono.Nome, result.Nome);
        Assert.Equal(dono.Telefone, result.Telefone);
        Assert.Equal(dono.Email, result.Email);

        repositoryMock.Verify(
            repository => repository.GetByIdAsync(dono.DonoId),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_QuandoDonoNaoExiste_DeveLancarKeyNotFoundException()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Dono>>();

        var donoId = Guid.NewGuid();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(donoId))
            .ReturnsAsync((Dono?)null);

        var service = new DonoService(repositoryMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.GetByIdAsync(donoId));

        // Assert
        Assert.Contains(donoId.ToString(), exception.Message);

        repositoryMock.Verify(
            repository => repository.GetByIdAsync(donoId),
            Times.Once);

        repositoryMock.Verify(
            repository => repository.AddAsync(It.IsAny<Dono>()),
            Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosOsDonos()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Dono>>();

        var donos = new List<Dono>
        {
            new()
            {
                DonoId = Guid.NewGuid(),
                Nome = "João Silva",
                Telefone = "11977777777",
                Email = "joao@email.com"
            },
            new()
            {
                DonoId = Guid.NewGuid(),
                Nome = "Ana Souza",
                Telefone = "11966666666",
                Email = "ana@email.com"
            }
        };

        repositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(donos);

        var service = new DonoService(repositoryMock.Object);

        // Act
        var result = (await service.GetAllAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal(donos[0].DonoId, result[0].DonoId);
        Assert.Equal(donos[0].Nome, result[0].Nome);

        Assert.Equal(donos[1].DonoId, result[1].DonoId);
        Assert.Equal(donos[1].Nome, result[1].Nome);

        repositoryMock.Verify(
            repository => repository.GetAllAsync(),
            Times.Once);
    }
}