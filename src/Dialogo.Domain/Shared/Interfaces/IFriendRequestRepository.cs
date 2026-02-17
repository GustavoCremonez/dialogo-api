using Dialogo.Domain.Entities;

namespace Dialogo.Domain.Shared.Interfaces;

public interface IFriendRequestRepository : IRepository<FriendRequest>
{
    Task<bool> ExistsAsync(Guid fromUserId, Guid toUserId, CancellationToken cancellationToken);
}
