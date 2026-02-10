namespace Dialogo.Application.Features.Auth.Shared;

/// <summary>
/// Response com tokens de autenticação
/// </summary>
public record AuthResponse(
    /// <summary>
    /// Token JWT de acesso (válido por 30 minutos)
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    string AccessToken,

    /// <summary>
    /// Token de renovação (válido por 7 dias)
    /// </summary>
    /// <example>abc123def456...</example>
    string RefreshToken,

    /// <summary>
    /// Data/hora de expiração do access token
    /// </summary>
    /// <example>2025-02-10T23:00:00Z</example>
    DateTime ExpiresAt);
