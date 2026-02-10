using Dialogo.Api.Extensions;
using Dialogo.Application.Features.Auth.Login;
using Dialogo.Application.Features.Auth.Logout;
using Dialogo.Application.Features.Auth.Refresh;
using Dialogo.Application.Features.Auth.Register;
using Dialogo.Application.Features.Auth.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dialogo.Api.Controllers;

/// <summary>
/// Gerenciamento de autenticação e autorização
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly RegisterHandler _registerHandler;
    private readonly LoginHandler _loginHandler;
    private readonly RefreshTokenHandler _refreshTokenHandler;
    private readonly LogoutHandler _logoutHandler;

    public AuthController(
        RegisterHandler registerHandler,
        LoginHandler loginHandler,
        RefreshTokenHandler refreshTokenHandler,
        LogoutHandler logoutHandler)
    {
        _registerHandler = registerHandler;
        _loginHandler = loginHandler;
        _refreshTokenHandler = refreshTokenHandler;
        _logoutHandler = logoutHandler;
    }

    /// <summary>
    /// Registra um novo usuário no sistema
    /// </summary>
    /// <param name="request">Dados do novo usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Tokens de autenticação</returns>
    /// <response code="201">Usuário criado com sucesso</response>
    /// <response code="400">Dados inválidos (validação falhou)</response>
    /// <response code="409">Email já está em uso</response>
    /// <response code="429">Muitas tentativas (rate limit excedido)</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _registerHandler.Handle(request, cancellationToken);
        return result.ToCreatedResult();
    }

    /// <summary>
    /// Autentica um usuário existente
    /// </summary>
    /// <param name="request">Credenciais de login</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Tokens de autenticação</returns>
    /// <response code="200">Login realizado com sucesso</response>
    /// <response code="400">Dados inválidos (validação falhou)</response>
    /// <response code="401">Credenciais inválidas</response>
    /// <response code="403">Usuário inativo</response>
    /// <response code="429">Muitas tentativas (rate limit excedido)</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _loginHandler.Handle(request, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Renova o token de acesso usando refresh token
    /// </summary>
    /// <param name="request">Refresh token atual</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Novos tokens de autenticação</returns>
    /// <response code="200">Tokens renovados com sucesso</response>
    /// <response code="400">Dados inválidos (validação falhou)</response>
    /// <response code="401">Refresh token inválido ou expirado</response>
    /// <response code="403">Usuário inativo</response>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _refreshTokenHandler.Handle(request, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Revoga o refresh token (logout)
    /// </summary>
    /// <param name="request">Refresh token a ser revogado</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Logout realizado com sucesso</response>
    /// <response code="400">Dados inválidos (validação falhou)</response>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken cancellationToken)
    {
        var result = await _logoutHandler.Handle(request, cancellationToken);
        return result.ToNoContentResult();
    }

    /// <summary>
    /// Obtém informações do usuário autenticado
    /// </summary>
    /// <returns>Dados do usuário</returns>
    /// <response code="200">Informações retornadas com sucesso</response>
    /// <response code="401">Não autenticado ou token inválido</response>
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Me()
    {
        var userId = User.FindFirst("sub")?.Value;
        var email = User.FindFirst("email")?.Value;
        var name = User.FindFirst("name")?.Value;

        return Ok(new
        {
            UserId = userId,
            Email = email,
            Name = name
        });
    }
}
