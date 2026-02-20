using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;

namespace Dialogo.Infrastructure.Persistence.Repositories;

public class FriendshipRepository : Repository<Friendship>, IFriendshipRepository
{
    public FriendshipRepository(ApplicationDbContext context) : base(context)
    {
    }
}
