using Microsoft.EntityFrameworkCore;
using PetLyve.Application;
using PetLyve.Application.Pagination;
using PetLyve.Domain.Entities.Base;

namespace PetLyve.Infrastructure.Data.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<PagedResult<T>> GetPagedAsync(
        PageRequest request,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(orderBy);

        var query = _dbSet.AsNoTracking();

        var totalItems = await query.CountAsync();

        // long evita overflow em páginas muito altas; além do total a
        // página é simplesmente vazia (não é erro).
        var skip = (long)(request.Page - 1) * request.PageSize;

        if (skip >= totalItems)
        {
            return new PagedResult<T>([], totalItems);
        }

        var items = await orderBy(query)
            .Skip((int)skip)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResult<T>(items, totalItems);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity is not null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity is not null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity is not null;
    }
}