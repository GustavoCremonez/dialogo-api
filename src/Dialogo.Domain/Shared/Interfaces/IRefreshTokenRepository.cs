using Dialogo.Domain.Entities;

namespace Dialogo.Domain.Shared.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
    Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
