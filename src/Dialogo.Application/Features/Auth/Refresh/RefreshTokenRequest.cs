using System.ComponentModel.DataAnnotations;

namespace Dialogo.Application.Features.Auth.Refresh;

/// <summary>
/// Request para renovação de token de acesso
/// </summary>
public record RefreshTokenRequest(
    /// <summary>
    /// Refresh token obtido no login ou registro
    /// </summary>
    /// <example>abc123def456...</example>
    [Required]
    string RefreshToken);
