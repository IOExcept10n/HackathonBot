using Microsoft.EntityFrameworkCore;

namespace MyBots.Persistence.Repository;

public class Repository<TContext, T>(TContext context) : IRepository<T> 
    where TContext : DbContext
    where T : class
{
    protected readonly TContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public virtual IQueryable<T> GetAll() => _dbSet;

    public virtual async Task<T?> GetByIdAsync<TKey>(TKey id) => await _dbSet.FindAsync(id);

    public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public virtual Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}