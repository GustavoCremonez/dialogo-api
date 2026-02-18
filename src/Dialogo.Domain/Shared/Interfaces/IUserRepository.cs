using Dialogo.Domain.Entities;

namespace Dialogo.Domain.Shared.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByPublicCodeAsync(string publicCode, CancellationToken cancellationToken);
}
