namespace PetLyve.Application.Pagination;

/// <summary>
/// Pedido de página já validado. Só é possível obter uma instância por
/// <see cref="Create"/>, que garante page &gt;= 1 e pageSize entre 1 e
/// <see cref="MaxPageSize"/>.
/// </summary>
public sealed record PageRequest
{
    public const int DefaultPage = 1;

    public const int DefaultPageSize = 20;

    public const int MaxPageSize = 100;

    private PageRequest(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public int Page { get; }

    public int PageSize { get; }

    public static PageRequest Create(int page, int pageSize)
    {
        var errors = new Dictionary<string, string[]>();

        if (page < 1)
        {
            errors["page"] =
            [
                $"O parâmetro 'page' deve ser um inteiro maior ou igual a 1. Valor recebido: {page}."
            ];
        }

        if (pageSize < 1 || pageSize > MaxPageSize)
        {
            errors["pageSize"] =
            [
                $"O parâmetro 'pageSize' deve ser um inteiro entre 1 e {MaxPageSize}. Valor recebido: {pageSize}."
            ];
        }

        if (errors.Count > 0)
        {
            throw new InvalidPaginationException(errors);
        }

        return new PageRequest(page, pageSize);
    }
}
