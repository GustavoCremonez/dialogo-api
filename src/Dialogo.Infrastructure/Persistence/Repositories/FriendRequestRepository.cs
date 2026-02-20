using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dialogo.Infrastructure.Persistence.Repositories;

public class FriendRequestRepository : Repository<FriendRequest>, IFriendRequestRepository
{
    public FriendRequestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<FriendRequest>> GetFriendRequestByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var friendRequests = await Context.FriendRequests
            .AsNoTracking()
            .Include(fr => fr.ToUser)
            .Where(fr => fr.FromUserId == userId)
            .ToListAsync(cancellationToken);

        return friendRequests;
    }
}
