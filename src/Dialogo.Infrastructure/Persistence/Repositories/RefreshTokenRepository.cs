using System.Linq.Expressions;
using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dialogo.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<RefreshToken>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<RefreshToken>> FindAsync(Expression<Func<RefreshToken, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens.Where(predicate).ToListAsync(cancellationToken);
    }

    public void Add(RefreshToken entity)
    {
        _context.RefreshTokens.Add(entity);
    }

    public void Update(RefreshToken entity)
    {
        _context.RefreshTokens.Update(entity);
    }

    public void Remove(RefreshToken entity)
    {
        _context.RefreshTokens.Remove(entity);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > now)
            .ToListAsync(cancellationToken);
    }

    public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.Revoke();
        }
    }
}
