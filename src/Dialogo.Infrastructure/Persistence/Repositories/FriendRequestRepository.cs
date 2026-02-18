using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dialogo.Infrastructure.Persistence.Repositories;

public class FriendRequestRepository : IFriendRequestRepository
{
    private readonly ApplicationDbContext _context;

    public FriendRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FriendRequest entity)
    {
        await _context.FriendRequests.AddAsync(entity);
    }

    public void Update(FriendRequest entity)
    {
        _context.FriendRequests.Update(entity);
    }

    public void Remove(FriendRequest entity)
    {
        _context.FriendRequests.Remove(entity);
    }

    public async Task<IEnumerable<FriendRequest>> FindAsync(Expression<Func<FriendRequest, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.FriendRequests.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FriendRequest>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.FriendRequests.ToListAsync(cancellationToken);
    }

    public async Task<FriendRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.FriendRequests.FindAsync([id], cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid fromUserId, Guid toUserId, CancellationToken cancellationToken)
    {
        var friendRequest = await _context.FriendRequests
            .FirstOrDefaultAsync(fr => fr.FromUserId == fromUserId &&
                fr.ToUserId == toUserId &&
                fr.Status == Domain.Enums.FriendRequestStatus.Pending);

        return friendRequest is not null;
    }
}
