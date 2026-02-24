using Dialogo.Domain.Entities;
using System.Linq.Expressions;

namespace Dialogo.Domain.Shared.Interfaces;

public interface IFriendshipRepository : IRepository<Friendship>
{
    Task<IEnumerable<TProjection>> GetFriendsByUserAsync<TProjection>(
        Guid userId,
        Expression<Func<Friendship, TProjection>> selectAsOwner,
        Expression<Func<Friendship, TProjection>> selectAsFriend,
        CancellationToken cancellationToken);
}
