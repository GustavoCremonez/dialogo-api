using Dialogo.Domain.Entities;
using System.Linq.Expressions;

namespace Dialogo.Domain.Shared.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<TProjection?> GetProjectionByPublicCodeAsync<TProjection>(
        string publicCode,
        Expression<Func<User, TProjection>> projection,
        CancellationToken cancellationToken) where TProjection : class;
}
