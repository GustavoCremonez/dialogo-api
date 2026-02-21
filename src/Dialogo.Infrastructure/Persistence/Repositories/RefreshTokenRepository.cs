using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dialogo.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await Context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        return await Context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > now)
            .ToListAsync(cancellationToken);
    }

    public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var tokens = await Context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.Revoke();
        }
    }
}
