using PetLyve.Application.DTOs;
using PetLyve.Application.Pagination;

namespace PetLyve.Application.Tests.Pagination;

public class PageRequestTests
{
    [Theory]
    [InlineData(0, 20, "page")]
    [InlineData(-1, 20, "page")]
    [InlineData(1, 0, "pageSize")]
    [InlineData(1, -5, "pageSize")]
    [InlineData(1, 101, "pageSize")]
    [InlineData(1, 9999, "pageSize")]
    public void Create_ComValoresForaDoIntervalo_DeveLancarInvalidPaginationException(
        int page,
        int pageSize,
        string parametroInvalido)
    {
        // Act
        var exception = Assert.Throws<InvalidPaginationException>(
            () => PageRequest.Create(page, pageSize));

        // Assert
        Assert.Single(exception.Errors);
        Assert.True(exception.Errors.ContainsKey(parametroInvalido));
        Assert.Contains(parametroInvalido, exception.Message);
    }

    [Fact]
    public void Create_ComPageEPageSizeInvalidos_DeveReportarAsDuasRegras()
    {
        // Act
        var exception = Assert.Throws<InvalidPaginationException>(
            () => PageRequest.Create(0, 9999));

        // Assert
        Assert.Equal(2, exception.Errors.Count);
        Assert.True(exception.Errors.ContainsKey("page"));
        Assert.True(exception.Errors.ContainsKey("pageSize"));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 20)]
    [InlineData(5, 100)]
    [InlineData(int.MaxValue, 100)]
    public void Create_ComValoresNoIntervalo_DeveCriarPedidoDePagina(
        int page,
        int pageSize)
    {
        // Act
        var request = PageRequest.Create(page, pageSize);

        // Assert
        Assert.Equal(page, request.Page);
        Assert.Equal(pageSize, request.PageSize);
    }

    [Fact]
    public void Create_ComValoresPadrao_DeveUsarPagina1ETamanho20()
    {
        // Act
        var request = PageRequest.Create(
            PageRequest.DefaultPage,
            PageRequest.DefaultPageSize);

        // Assert
        Assert.Equal(1, request.Page);
        Assert.Equal(20, request.PageSize);
    }

    [Theory]
    [InlineData(0, 20, 0)]
    [InlineData(1, 20, 1)]
    [InlineData(20, 20, 1)]
    [InlineData(21, 20, 2)]
    [InlineData(137, 20, 7)]
    [InlineData(5, 2, 3)]
    public void PagedResponse_DeveCalcularTotalPagesComTeto(
        int totalItems,
        int pageSize,
        int totalPagesEsperado)
    {
        // Arrange
        var request = PageRequest.Create(1, pageSize);

        // Act
        var response = PagedResponseDto<string>.Create(request, totalItems, []);

        // Assert
        Assert.Equal(totalPagesEsperado, response.TotalPages);
    }
}
