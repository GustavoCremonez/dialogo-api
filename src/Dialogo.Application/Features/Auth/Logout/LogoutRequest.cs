using System.ComponentModel.DataAnnotations;

namespace Dialogo.Application.Features.Auth.Logout;

/// <summary>
/// Request para logout (revogação de refresh token)
/// </summary>
public record LogoutRequest(
    /// <summary>
    /// Refresh token a ser revogado
    /// </summary>
    /// <example>abc123def456...</example>
    [Required]
    string RefreshToken);
