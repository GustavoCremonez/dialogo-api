using System.ComponentModel.DataAnnotations;

namespace Dialogo.Application.Features.Auth.Register;

/// <summary>
/// Request para registro de novo usuário
/// </summary>
public record RegisterRequest(
    /// <summary>
    /// Email do usuário (único no sistema)
    /// </summary>
    /// <example>user@example.com</example>
    [Required]
    string Email,

    /// <summary>
    /// Senha (mínimo 8 caracteres, deve conter maiúscula, minúscula, número e caractere especial)
    /// </summary>
    /// <example>Password123!</example>
    [Required]
    string Password,

    /// <summary>
    /// Nome completo do usuário
    /// </summary>
    /// <example>John Doe</example>
    [Required]
    string Name);
