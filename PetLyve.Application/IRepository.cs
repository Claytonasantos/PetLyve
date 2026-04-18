namespace PetLyve.Application;

public interface IRepository<T> where T : class
{
    Task<T?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<T>> ObterTodosAsync();
    Task AdicionarAsync(T entity);
    Task SalvarAlteracoesAsync();
}