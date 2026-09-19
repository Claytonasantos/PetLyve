using PetLyve.Domain.Entities.Base;

namespace PetLyve.Application;

public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id);

    Task AddAsync(T entity);

    Task DeleteAsync(T entity);

    Task<bool> ExistsByIdAsync(Guid id);
}