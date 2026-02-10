using Dialogo.Application.Features.Auth.Register;
using FluentValidation.TestHelper;

namespace Dialogo.Application.Tests.Features.Auth.Register;

public class RegisterValidatorTests
{
    private readonly RegisterValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var request = new RegisterRequest("", "Password123!", "John Doe");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var request = new RegisterRequest("invalid-email", "Password123!", "John Doe");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var request = new RegisterRequest("test@example.com", "Pass1!", "John Doe");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Uppercase()
    {
        var request = new RegisterRequest("test@example.com", "password123!", "John Doe");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Lowercase()
    {
        var request = new RegisterRequest("test@example.com", "PASSWORD123!", "John Doe");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Number()
    {
        var request = new RegisterRequest("test@example.com", "Password!!", "John Doe");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Special_Character()
    {
        var request = new RegisterRequest("test@example.com", "Password123", "John Doe");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var request = new RegisterRequest("test@example.com", "Password123!", "");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Short()
    {
        var request = new RegisterRequest("test@example.com", "Password123!", "J");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid_Request()
    {
        var request = new RegisterRequest("test@example.com", "Password123!", "John Doe");
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
