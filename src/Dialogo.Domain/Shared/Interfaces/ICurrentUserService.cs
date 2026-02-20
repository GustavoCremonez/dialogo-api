namespace Dialogo.Domain.Shared.Interfaces;

/// <summary>
/// Serviço para obter informações do usuário autenticado
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Obtém o ID do usuário autenticado
    /// </summary>
    /// <returns>ID do usuário</returns>
    /// <exception cref="UnauthorizedAccessException">Quando o usuário não está autenticado ou claim não existe</exception>
    Guid GetUserId();

    /// <summary>
    /// Obtém o email do usuário autenticado
    /// </summary>
    /// <returns>Email do usuário ou null se não existir</returns>
    string? GetUserEmail();

    /// <summary>
    /// Obtém o nome do usuário autenticado
    /// </summary>
    /// <returns>Nome do usuário ou null se não existir</returns>
    string? GetUserName();

    /// <summary>
    /// Verifica se há um usuário autenticado
    /// </summary>
    /// <returns>True se autenticado, false caso contrário</returns>
    bool IsAuthenticated();
}
