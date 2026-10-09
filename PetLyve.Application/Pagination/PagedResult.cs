namespace PetLyve.Application.Pagination;

/// <summary>
/// Resultado bruto de uma consulta paginada no repositório:
/// os itens da página e o total de registros da consulta.
/// </summary>
public sealed record PagedResult<T>(IReadOnlyList<T> Items, int TotalItems);
