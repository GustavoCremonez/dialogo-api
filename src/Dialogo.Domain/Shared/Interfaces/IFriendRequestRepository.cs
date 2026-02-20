using Dialogo.Domain.Entities;

namespace Dialogo.Domain.Shared.Interfaces;

public interface IFriendRequestRepository : IRepository<FriendRequest>
{
    Task<IEnumerable<FriendRequest>> GetFriendRequestByUserAsync(Guid userId, CancellationToken cancellationToken);
}
