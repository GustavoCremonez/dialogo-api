using Dialogo.Api.Extensions;
using Dialogo.Application.Features.Friend.AcceptFriendRequest;
using Dialogo.Application.Features.Friend.GetFriends;
using Dialogo.Application.Features.Friend.GetReceivedRequests;
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
    private readonly GetFriendRequestsHandler _getFriendsHandler;
    private readonly GetReceivedFriendRequestsHandler _getReceivedFriendRequestsHandler;

    public FriendController(
        SendFriendRequestHandler sendFriendRequestHandler,
        AcceptFriendRequestHandler acceptFriendRequestHandler,
        RejectFriendRequestHandler rejectFriendRequestHandler,
        GetFriendRequestsHandler getFriendsHandler,
        GetReceivedFriendRequestsHandler getReceivedFriendRequestsHandler)
    {
        _sendFriendRequestHandler = sendFriendRequestHandler;
        _acceptFriendRequestHandler = acceptFriendRequestHandler;
        _rejectFriendRequestHandler = rejectFriendRequestHandler;
        _getFriendsHandler = getFriendsHandler;
        _getReceivedFriendRequestsHandler = getReceivedFriendRequestsHandler;
    }

    /// <summary>
    /// Envia solicitação de amizade para o usuário identificado pelo código público.
    /// </summary>
    /// <param name="request">Objeto contendo o código público do usuário destino</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <response code="201">Solicitação enviada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="401">Não autenticado ou token inválido</response>
    /// <response code="403">Usuário inativo</response>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SendRequest([FromBody] SendFriendRequestRequest request, CancellationToken cancellationToken)
    {
        var result = await _sendFriendRequestHandler.Handle(request, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Aceita uma solicitação de amizade recebida
    /// </summary>
    /// <param name="requestId">Identificador da solicitação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <response code="200">Solicitação aceita com sucesso</response>
    /// <response code="401">Não autenticado ou token inválido</response>
    /// <response code="403">Sem permissão</response>
    /// <response code="404">Solicitação não encontrada</response>
    [HttpPatch("requests/{requestId:guid}/accept")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AcceptRequest([FromRoute] Guid requestId, CancellationToken cancellationToken)
    {
        var result = await _acceptFriendRequestHandler.Handle(new AcceptFriendRequestRequest(requestId), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Rejeita uma solicitação de amizade recebida
    /// </summary>
    /// <param name="requestId">Identificador da solicitação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <response code="200">Solicitação rejeitada com sucesso</response>
    /// <response code="401">Não autenticado ou token inválido</response>
    /// <response code="403">Sem permissão</response>
    /// <response code="404">Solicitação não encontrada</response>
    [HttpPatch("requests/{requestId:guid}/reject")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RejectRequest([FromRoute] Guid requestId, CancellationToken cancellationToken)
    {
        var result = await _rejectFriendRequestHandler.Handle(new RejectFriendRequestRequest(requestId), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Obtém a lista de amigos do usuário autenticado
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <response code="200">Lista de amigos retornada com sucesso</response>
    /// <response code="401">Não autenticado ou token inválido</response>
    [HttpGet("friends")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFriends(CancellationToken cancellationToken)
    {
        var result = await _getFriendsHandler.Handle(cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Obtém as solicitações de amizade pendentes recebidas pelo usuário autenticado
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <response code="200">Solicitações retornadas com sucesso</response>
    /// <response code="401">Não autenticado ou token inválido</response>
    [HttpGet("requests/received")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetReceivedRequests(CancellationToken cancellationToken)
    {
        var result = await _getReceivedFriendRequestsHandler.Handle(cancellationToken);
        return result.ToActionResult();
    }
}
