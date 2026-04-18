using Microsoft.EntityFrameworkCore;
using PetLyve.Application;

namespace PetLyve.Infrastructure.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> ObterPorIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> ObterTodosAsync() => await _dbSet.ToListAsync();

    public async Task AdicionarAsync(T entity) => await _dbSet.AddAsync(entity);

    public async Task SalvarAlteracoesAsync() => await _context.SaveChangesAsync();
}