using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public async Task<TProjection?> GetProjectionByPublicCodeAsync<TProjection>(
        string publicCode,
        Expression<Func<User, TProjection>> projection,
        CancellationToken cancellationToken) where TProjection : class
    {
        return await Context.Users
            .AsNoTracking()
            .Where(u => u.PublicCode == publicCode)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
