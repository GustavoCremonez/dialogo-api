using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dialogo.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await Context.Users
            .FirstOrDefaultAsync(u => u.Email == email.ToLower(), cancellationToken);
    }

    public async Task<User?> GetByPublicCodeAsync(string publicCode, CancellationToken cancellationToken)
    {
        return await Context.Users
            .SingleOrDefaultAsync(u => u.PublicCode == publicCode, cancellationToken);
    }
}
