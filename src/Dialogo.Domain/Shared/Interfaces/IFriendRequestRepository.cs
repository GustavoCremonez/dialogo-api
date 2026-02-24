using Dialogo.Domain.Entities;
using System.Linq.Expressions;

namespace Dialogo.Domain.Shared.Interfaces;

public interface IFriendRequestRepository : IRepository<FriendRequest>
{
    Task<IEnumerable<TProjection>> GetByUserProjectionAsync<TProjection>(
        Guid userId,
        Expression<Func<FriendRequest, TProjection>> projection,
        CancellationToken cancellationToken);

    Task<IEnumerable<TProjection>> GetReceivedPendingProjectionAsync<TProjection>(
        Guid userId,
        Expression<Func<FriendRequest, TProjection>> projection,
        CancellationToken cancellationToken);
}
