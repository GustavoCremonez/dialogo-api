using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dialogo.Infrastructure.Persistence.Repositories;

public class FriendshipRepository : Repository<Friendship>, IFriendshipRepository
{
    public FriendshipRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TProjection>> GetFriendsByUserAsync<TProjection>(
        Guid userId,
        Expression<Func<Friendship, TProjection>> selectAsOwner,
        Expression<Func<Friendship, TProjection>> selectAsFriend,
        CancellationToken cancellationToken)
    {
        var asOwner = await Context.Friendships
            .AsNoTracking()
            .Where(f => f.UserId == userId)
            .Select(selectAsOwner)
            .ToListAsync(cancellationToken);

        var asFriend = await Context.Friendships
            .AsNoTracking()
            .Where(f => f.FriendUserId == userId)
            .Select(selectAsFriend)
            .ToListAsync(cancellationToken);

        return asOwner.Concat(asFriend);
    }
}
