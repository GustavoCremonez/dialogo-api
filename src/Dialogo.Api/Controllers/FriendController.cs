using Dialogo.Api.Extensions;
using Dialogo.Application.Features.Auth.Shared;
using Dialogo.Application.Features.Friend.FriendRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Dialogo.Api.Controllers;

/// <summary>
/// Gerenciamento de amizade
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class FriendController : ControllerBase
{
    private readonly SendFriendRequestHandler _sendFriendRequestHandler;

    public FriendController(SendFriendRequestHandler sendFriendRequestHandler)
    {
        _sendFriendRequestHandler = sendFriendRequestHandler;
    }

    /// <summary>
    /// Envia solicitação de amizade para o usuário identificado pelo código público.
    /// </summary>
    /// <param name="request">Objeto contendo o código público do usuário destino</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <response code="201">Quando a solicitação foi enviada (implementação básica)</response>
    /// <response code="401">Credenciais inválidas</response>
    /// <response code="403">Usuário inativo</response>
    /// <response code="429">Muitas tentativas (rate limit excedido)</response>
    [Authorize]
    [HttpPost("requests")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> SendRequest([FromBody] SendFriendRequestRequest request, CancellationToken cancellationToken)
    {
        var fromUserId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        var result = await _sendFriendRequestHandler.Handle(request, fromUserId, cancellationToken);
        return result.ToActionResult();
    }
}
