using Dialogo.Domain.Entities;

namespace Dialogo.Domain.Tests.Entities;

public class RefreshTokenTests
{
    [Fact]
    public void Create_Should_Create_RefreshToken_With_Valid_Data()
    {
        var userId = Guid.NewGuid();
        var token = "test_token";
        var expiresAt = DateTime.UtcNow.AddDays(7);

        var refreshToken = RefreshToken.Create(userId, token, expiresAt);

        Assert.NotEqual(Guid.Empty, refreshToken.Id);
        Assert.Equal(userId, refreshToken.UserId);
        Assert.Equal(token, refreshToken.Token);
        Assert.Equal(expiresAt, refreshToken.ExpiresAt);
        Assert.False(refreshToken.IsRevoked);
        Assert.False(refreshToken.IsExpired);
        Assert.True(refreshToken.IsActive);
    }

    [Fact]
    public void Create_Should_Throw_When_UserId_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() =>
            RefreshToken.Create(Guid.Empty, "token", DateTime.UtcNow.AddDays(7)));
    }

    [Fact]
    public void Create_Should_Throw_When_Token_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() =>
            RefreshToken.Create(Guid.NewGuid(), "", DateTime.UtcNow.AddDays(7)));
    }

    [Fact]
    public void Create_Should_Throw_When_ExpiresAt_Is_In_Past()
    {
        Assert.Throws<ArgumentException>(() =>
            RefreshToken.Create(Guid.NewGuid(), "token", DateTime.UtcNow.AddDays(-1)));
    }

    [Fact]
    public void Revoke_Should_Set_RevokedAt()
    {
        var refreshToken = RefreshToken.Create(Guid.NewGuid(), "token", DateTime.UtcNow.AddDays(7));

        refreshToken.Revoke();

        Assert.True(refreshToken.IsRevoked);
        Assert.NotNull(refreshToken.RevokedAt);
        Assert.False(refreshToken.IsActive);
    }

    [Fact]
    public void Revoke_Should_Throw_When_Already_Revoked()
    {
        var refreshToken = RefreshToken.Create(Guid.NewGuid(), "token", DateTime.UtcNow.AddDays(7));
        refreshToken.Revoke();

        Assert.Throws<InvalidOperationException>(() => refreshToken.Revoke());
    }

    [Fact]
    public void IsExpired_Should_Return_True_When_Token_Is_Expired()
    {
        var refreshToken = RefreshToken.Create(Guid.NewGuid(), "token", DateTime.UtcNow.AddMilliseconds(10));

        Thread.Sleep(20);

        Assert.True(refreshToken.IsExpired);
        Assert.False(refreshToken.IsActive);
    }
}
