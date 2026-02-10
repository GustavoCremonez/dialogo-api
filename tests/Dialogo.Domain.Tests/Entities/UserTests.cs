using Dialogo.Domain.Entities;

namespace Dialogo.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Create_Should_Create_User_With_Valid_Data()
    {
        var email = "test@example.com";
        var passwordHash = "hashed_password";
        var name = "John Doe";

        var user = User.Create(email, passwordHash, name);

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(email.ToLowerInvariant(), user.Email);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Equal(name, user.Name);
        Assert.True(user.IsActive);
        Assert.True((DateTime.UtcNow - user.CreatedAt).TotalSeconds < 1);
    }

    [Fact]
    public void Create_Should_Throw_When_Email_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() =>
            User.Create("", "hashed_password", "John Doe"));
    }

    [Fact]
    public void Create_Should_Throw_When_PasswordHash_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() =>
            User.Create("test@example.com", "", "John Doe"));
    }

    [Fact]
    public void Create_Should_Throw_When_Name_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() =>
            User.Create("test@example.com", "hashed_password", ""));
    }

    [Fact]
    public void UpdateLastLogin_Should_Update_LastLoginAt_And_UpdatedAt()
    {
        var user = User.Create("test@example.com", "hashed_password", "John Doe");
        var originalUpdatedAt = user.UpdatedAt;

        Thread.Sleep(10);
        user.UpdateLastLogin();

        Assert.NotNull(user.LastLoginAt);
        Assert.True(user.LastLoginAt > originalUpdatedAt);
        Assert.True(user.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void Deactivate_Should_Set_IsActive_To_False()
    {
        var user = User.Create("test@example.com", "hashed_password", "John Doe");

        user.Deactivate();

        Assert.False(user.IsActive);
    }

    [Fact]
    public void Activate_Should_Set_IsActive_To_True()
    {
        var user = User.Create("test@example.com", "hashed_password", "John Doe");
        user.Deactivate();

        user.Activate();

        Assert.True(user.IsActive);
    }
}
