namespace Dialogo.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;

    public User User { get; private set; } = null!;

    private RefreshToken() { }

    public static RefreshToken Create(Guid userId, string token, DateTime expiresAt)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("O ID do usuário é obrigatório.", nameof(userId));

        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("O token é obrigatório.", nameof(token));

        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("A data de expiração deve ser no futuro.", nameof(expiresAt));

        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Revoke()
    {
        if (IsRevoked)
            throw new InvalidOperationException("O token já foi revogado.");

        RevokedAt = DateTime.UtcNow;
    }
}
