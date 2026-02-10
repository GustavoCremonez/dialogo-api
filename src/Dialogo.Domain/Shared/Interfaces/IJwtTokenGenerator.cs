namespace Dialogo.Domain.Shared.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(Guid userId, string email, string name);
    string GenerateRefreshToken();
}
