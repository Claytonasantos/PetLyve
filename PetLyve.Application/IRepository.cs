using PetLyve.Application.Pagination;
using PetLyve.Domain.Entities.Base;

namespace PetLyve.Application;

public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Devolve uma página de registros. A contagem, a ordenação e o corte
    /// (Skip/Take) são executados no banco, antes da materialização.
    /// </summary>
    /// <param name="request">Página e tamanho de página já validados.</param>
    /// <param name="orderBy">
    /// Ordenação estável obrigatória (aplicada sobre o IQueryable), para que
    /// páginas consecutivas sejam reproduzíveis.
    /// </param>
    Task<PagedResult<T>> GetPagedAsync(
        PageRequest request,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);

    Task<T?> GetByIdAsync(Guid id);

    Task AddAsync(T entity);

    Task DeleteAsync(T entity);

    Task<bool> ExistsByIdAsync(Guid id);
}
