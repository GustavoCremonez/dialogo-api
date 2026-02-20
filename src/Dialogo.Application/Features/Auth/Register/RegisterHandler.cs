using Dialogo.Application.Features.Auth.Shared;
using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Dialogo.Domain.Shared.Results;
using Microsoft.Extensions.Configuration;

namespace Dialogo.Application.Features.Auth.Register;

public class RegisterHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public RegisterHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var emailExists = await _userRepository.ExistsAsync(u => u.Email == request.Email.ToLower(), cancellationToken);
        if (emailExists)
        {
            return Error.Conflict("User.EmailAlreadyExists", "Já existe um usuário com este email.");
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = User.Create(request.Email, passwordHash, request.Name);

        await _userRepository.AddAsync(user, cancellationToken);

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user.Id, user.Email, user.Name);
        var refreshTokenValue = _jwtTokenGenerator.GenerateRefreshToken();

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var refreshTokenExpirationDays = int.Parse(jwtSettings["RefreshTokenExpirationDays"] ?? "7");
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);

        var refreshToken = RefreshToken.Create(user.Id, refreshTokenValue, refreshTokenExpiresAt);
        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var accessTokenExpirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"] ?? "30");
        var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes);

        return new AuthResponse(accessToken, refreshTokenValue, accessTokenExpiresAt);
    }
}
