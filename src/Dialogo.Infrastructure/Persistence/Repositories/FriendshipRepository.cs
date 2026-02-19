using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dialogo.Infrastructure.Persistence.Repositories;

public class FriendshipRepository : IFriendshipRepository
{
    private readonly ApplicationDbContext _context;

    public FriendshipRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Friendship entity, CancellationToken cancellationToken)
    {
        await _context.Friendships.AddAsync(entity, cancellationToken);
    }

    public async Task<IEnumerable<Friendship>> FindAsync(Expression<Func<Friendship, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Friendships.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Friendship>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Friendships.ToListAsync(cancellationToken);
    }

    public async Task<Friendship?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Friendships.FindAsync([id], cancellationToken);
    }

    public void Remove(Friendship entity)
    {
        _context.Friendships.Remove(entity);
    }

    public void Update(Friendship entity)
    {
        _context.Friendships.Update(entity);
    }
}
