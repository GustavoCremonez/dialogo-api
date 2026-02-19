using Dialogo.Application.Features.Auth.Shared;
using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Dialogo.Domain.Shared.Results;
using Microsoft.Extensions.Configuration;

namespace Dialogo.Application.Features.Auth.Refresh;

public class RefreshTokenHandler
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public RefreshTokenHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponse>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (refreshToken is null || !refreshToken.IsActive)
        {
            return Error.Unauthorized("Auth.InvalidRefreshToken", "Refresh token inválido ou expirado.");
        }

        var user = refreshToken.User;

        if (!user.IsActive)
        {
            return Error.Forbidden("Auth.UserInactive", "Conta de usuário inativa.");
        }

        refreshToken.Revoke();

        var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(user.Id, user.Email, user.Name);
        var newRefreshTokenValue = _jwtTokenGenerator.GenerateRefreshToken();

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var refreshTokenExpirationDays = int.Parse(jwtSettings["RefreshTokenExpirationDays"] ?? "7");
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);

        var newRefreshToken = RefreshToken.Create(user.Id, newRefreshTokenValue, refreshTokenExpiresAt);
        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var accessTokenExpirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"] ?? "30");
        var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes);

        return new AuthResponse(newAccessToken, newRefreshTokenValue, accessTokenExpiresAt);
    }
}
