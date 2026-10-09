using PetLyve.Application.Pagination;

namespace PetLyve.Application.DTOs;

/// <summary>
/// Envelope paginado devolvido pelas listagens da API v2.
/// </summary>
public class PagedResponseDto<T>
{
    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalItems { get; init; }

    public int TotalPages { get; init; }

    public bool HasPrevious { get; init; }

    public bool HasNext { get; init; }

    public IReadOnlyList<T> Items { get; init; } = [];

    public static PagedResponseDto<T> Create(
        PageRequest request,
        int totalItems,
        IReadOnlyList<T> items)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

        return new PagedResponseDto<T>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            HasPrevious = request.Page > 1,
            HasNext = request.Page < totalPages,
            Items = items
        };
    }
}
