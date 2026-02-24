using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dialogo.Infrastructure.Persistence.Repositories;

public class FriendRequestRepository : Repository<FriendRequest>, IFriendRequestRepository
{
    public FriendRequestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TProjection>> GetByUserProjectionAsync<TProjection>(
        Guid userId,
        Expression<Func<FriendRequest, TProjection>> projection,
        CancellationToken cancellationToken)
    {
        return await Context.FriendRequests
            .AsNoTracking()
            .Where(fr => fr.FromUserId == userId)
            .Select(projection)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TProjection>> GetReceivedPendingProjectionAsync<TProjection>(
        Guid userId,
        Expression<Func<FriendRequest, TProjection>> projection,
        CancellationToken cancellationToken)
    {
        return await Context.FriendRequests
            .AsNoTracking()
            .Where(fr => fr.ToUserId == userId && fr.Status == Domain.Enums.FriendRequestStatus.Pending)
            .Select(projection)
            .ToListAsync(cancellationToken);
    }
}
