using System.ComponentModel.DataAnnotations;

namespace Dialogo.Application.Features.Auth.Login;

/// <summary>
/// Request para autenticação de usuário
/// </summary>
public record LoginRequest(
    /// <summary>
    /// Email do usuário
    /// </summary>
    /// <example>user@example.com</example>
    [Required]
    string Email,

    /// <summary>
    /// Senha do usuário
    /// </summary>
    /// <example>Password123!</example>
    [Required]
    string Password);
