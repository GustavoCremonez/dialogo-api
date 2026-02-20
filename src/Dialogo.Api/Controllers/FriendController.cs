using Dialogo.Api.Extensions;
using Dialogo.Application.Features.Auth.Shared;
using Dialogo.Application.Features.Friend.AcceptFriendRequest;
using Dialogo.Application.Features.Friend.GetFriends;
using Dialogo.Application.Features.Friend.RejectFriendRequest;
using Dialogo.Application.Features.Friend.SendFriendRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dialogo.Api.Controllers;

/// <summary>
/// Gerenciamento de amizade
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FriendController : ControllerBase
{
    private readonly SendFriendRequestHandler _sendFriendRequestHandler;
    private readonly AcceptFriendRequestHandler _acceptFriendRequestHandler;
    private readonly RejectFriendRequestHandler _rejectFriendRequestHandler;
    private readonly GetFriendRequestsHandler _getFriendRequestsHandler;

    public FriendController(
        SendFriendRequestHandler sendFriendRequestHandler,
        AcceptFriendRequestHandler acceptFriendRequestHandler,
        RejectFriendRequestHandler rejectFriendRequestHandler,
        GetFriendRequestsHandler getFriendsHandler)
    {
        _sendFriendRequestHandler = sendFriendRequestHandler;
        _acceptFriendRequestHandler = acceptFriendRequestHandler;
        _rejectFriendRequestHandler = rejectFriendRequestHandler;
        _getFriendRequestsHandler = getFriendsHandler;
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
    [HttpPost("requests")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> SendRequest([FromBody] SendFriendRequestRequest request, CancellationToken cancellationToken)
    {
        var result = await _sendFriendRequestHandler.Handle(request, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Aceita uma solicitação de amizade recebida
    /// </summary>
    /// <param name="request">Identificador da solicitação a ser aceita</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <response code="200">Solicitação aceita com sucesso</response>
    /// <response code="400">Dados inválidos (validação falhou)</response>
    /// <response code="401">Não autenticado ou token inválido</response>
    /// <response code="403">Usuário inativo ou sem permissão</response>
    /// <response code="429">Muitas tentativas (rate limit excedido)</response>
    [HttpPost("requests/accept")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AcceptRequest([FromBody] AcceptFriendRequestRequest request, CancellationToken cancellationToken)
    {
        var result = await _acceptFriendRequestHandler.Handle(request, cancellationToken);

        return result.ToActionResult();
    }

    /// <summary>
    /// Rejeita uma solicitação de amizade recebida
    /// </summary>
    /// <param name="request">Identificador da solicitação a ser rejeitada</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <response code="200">Solicitação rejeitada com sucesso</response>
    /// <response code="400">Dados inválidos (validação falhou)</response>
    /// <response code="401">Não autenticado ou token inválido</response>
    /// <response code="403">Usuário inativo ou sem permissão</response>
    /// <response code="429">Muitas tentativas (rate limit excedido)</response>
    [HttpPost("requests/reject")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RejectRequest([FromBody] RejectFriendRequestRequest request, CancellationToken cancellationToken)
    {
        var result = await _rejectFriendRequestHandler.Handle(request, cancellationToken);

        return result.ToActionResult();
    }

    /// <summary>
    /// Obtém a lista de amigos do usuário autenticado
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <response code="200">Lista de amigos retornada com sucesso</response>
    /// <response code="401">Não autenticado ou token inválido</response>
    /// <response code="429">Muitas tentativas (rate limit excedido)</response>
    [Authorize]
    [HttpGet("friend-requests")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFriendRequests(CancellationToken cancellationToken)
    {
        var result = await _getFriendRequestsHandler.Handle(cancellationToken);

        return result.ToActionResult();
    }
}
