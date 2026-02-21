using Dialogo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dialogo.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext Context;

    public Repository(ApplicationDbContext context)
    {
        Context = context;
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Set<T>().FindAsync([id], cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<T>().ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Context.Set<T>().Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Context.Set<T>().AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await Context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public virtual void Update(T entity)
    {
        Context.Set<T>().Update(entity);
    }

    public virtual void Remove(T entity)
    {
        Context.Set<T>().Remove(entity);
    }
}
