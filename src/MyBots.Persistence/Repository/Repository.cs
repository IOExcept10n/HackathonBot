using Microsoft.EntityFrameworkCore;

namespace MyBots.Core.Persistence.Repository;

/// <summary>
/// Provides a generic implementation of the repository pattern using Entity Framework Core.
/// </summary>
/// <typeparam name="TContext">The type of DbContext to use.</typeparam>
/// <typeparam name="T">The type of entity this repository works with.</typeparam>
/// <param name="context">The database context instance.</param>
public class Repository<TContext, T>(TContext context) : IRepository<T> 
    where TContext : DbContext
    where T : class
{
    /// <summary>
    /// Gets the database context instance.
    /// </summary>
    protected readonly TContext _context = context;

    /// <summary>
    /// Gets the DbSet for the entity type.
    /// </summary>
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    /// <inheritdoc />
    public virtual IQueryable<T> GetAll() => _dbSet;

    /// <inheritdoc />
    public virtual async Task<T?> GetByIdAsync<TKey>(TKey id) => await _dbSet.FindAsync(id);

    /// <inheritdoc />
    public virtual async Task AddAsync(T entity, CancellationToken ct = default) => await _dbSet.AddAsync(entity, ct);

    /// <inheritdoc />
    public virtual Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual async Task SaveChangesAsync(CancellationToken ct = default) => await _context.SaveChangesAsync(ct);

    /// <inheritdoc />
    public virtual async Task TruncateAsync(CancellationToken ct = default) => await _dbSet.ExecuteDeleteAsync(ct);
}