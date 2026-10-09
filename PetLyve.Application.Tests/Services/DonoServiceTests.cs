using Moq;
using PetLyve.Application.DTOs.Dono;
using PetLyve.Application.Pagination;
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

    [Fact]
    public async Task GetPagedAsync_ComIntervaloValido_DeveDevolverEnvelopeComTotais()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Dono>>();

        var donosDaPagina = new List<Dono>
        {
            new()
            {
                DonoId = Guid.NewGuid(),
                Nome = "Carla Lima",
                Telefone = "11955555555",
                Email = "carla@email.com"
            },
            new()
            {
                DonoId = Guid.NewGuid(),
                Nome = "Diego Rocha",
                Telefone = "11944444444",
                Email = "diego@email.com"
            }
        };

        repositoryMock
            .Setup(repository => repository.GetPagedAsync(
                It.IsAny<PageRequest>(),
                It.IsAny<Func<IQueryable<Dono>, IOrderedQueryable<Dono>>>()))
            .ReturnsAsync(new PagedResult<Dono>(donosDaPagina, 5));

        var service = new DonoService(repositoryMock.Object);

        // Act
        var result = await service.GetPagedAsync(page: 2, pageSize: 2);

        // Assert
        Assert.Equal(2, result.Page);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(5, result.TotalItems);
        Assert.Equal(3, result.TotalPages);
        Assert.True(result.HasPrevious);
        Assert.True(result.HasNext);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(donosDaPagina[0].DonoId, result.Items[0].DonoId);
        Assert.Equal(donosDaPagina[1].DonoId, result.Items[1].DonoId);

        repositoryMock.Verify(
            repository => repository.GetPagedAsync(
                It.Is<PageRequest>(request =>
                    request.Page == 2 &&
                    request.PageSize == 2),
                It.IsAny<Func<IQueryable<Dono>, IOrderedQueryable<Dono>>>()),
            Times.Once);

        repositoryMock.Verify(
            repository => repository.GetAllAsync(),
            Times.Never);
    }

    [Fact]
    public async Task GetPagedAsync_DeveOrdenarPorNomeEDesempatarPorId()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Dono>>();

        Func<IQueryable<Dono>, IOrderedQueryable<Dono>>? orderBy = null;

        repositoryMock
            .Setup(repository => repository.GetPagedAsync(
                It.IsAny<PageRequest>(),
                It.IsAny<Func<IQueryable<Dono>, IOrderedQueryable<Dono>>>()))
            .Callback<PageRequest, Func<IQueryable<Dono>, IOrderedQueryable<Dono>>>(
                (_, ordering) => orderBy = ordering)
            .ReturnsAsync(new PagedResult<Dono>([], 0));

        var service = new DonoService(repositoryMock.Object);

        var idMenor = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var idMaior = Guid.Parse("00000000-0000-0000-0000-000000000002");

        var donos = new List<Dono>
        {
            new() { DonoId = idMaior, Nome = "Bruno" },
            new() { DonoId = Guid.NewGuid(), Nome = "Zélia" },
            new() { DonoId = idMenor, Nome = "Bruno" },
            new() { DonoId = Guid.NewGuid(), Nome = "Ana" }
        };

        // Act
        await service.GetPagedAsync(page: 1, pageSize: 20);
        var ordenados = orderBy!(donos.AsQueryable()).ToList();

        // Assert
        Assert.Equal("Ana", ordenados[0].Nome);
        Assert.Equal(idMenor, ordenados[1].DonoId);
        Assert.Equal(idMaior, ordenados[2].DonoId);
        Assert.Equal("Zélia", ordenados[3].Nome);
    }

    [Fact]
    public async Task GetPagedAsync_ComPaginaAlemDoTotal_DeveDevolverItensVazios()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Dono>>();

        repositoryMock
            .Setup(repository => repository.GetPagedAsync(
                It.IsAny<PageRequest>(),
                It.IsAny<Func<IQueryable<Dono>, IOrderedQueryable<Dono>>>()))
            .ReturnsAsync(new PagedResult<Dono>([], 5));

        var service = new DonoService(repositoryMock.Object);

        // Act
        var result = await service.GetPagedAsync(page: 999, pageSize: 2);

        // Assert
        Assert.Empty(result.Items);
        Assert.Equal(5, result.TotalItems);
        Assert.Equal(3, result.TotalPages);
        Assert.False(result.HasNext);
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 0)]
    [InlineData(1, 9999)]
    public async Task GetPagedAsync_ComIntervaloInvalido_NaoDeveConsultarRepositorio(
        int page,
        int pageSize)
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Dono>>();

        var service = new DonoService(repositoryMock.Object);

        // Act
        await Assert.ThrowsAsync<InvalidPaginationException>(
            () => service.GetPagedAsync(page, pageSize));

        // Assert
        repositoryMock.Verify(
            repository => repository.GetPagedAsync(
                It.IsAny<PageRequest>(),
                It.IsAny<Func<IQueryable<Dono>, IOrderedQueryable<Dono>>>()),
            Times.Never);
    }
}