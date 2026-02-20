using Dialogo.Application.Features.Friend.Shared;
using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Dialogo.Domain.Shared.Results;

namespace Dialogo.Application.Features.Friend.AcceptFriendRequest;

public class AcceptFriendRequestHandler
{
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public AcceptFriendRequestHandler(
        IFriendRequestRepository friendRequestRepository,
        IFriendshipRepository friendshipRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _friendRequestRepository = friendRequestRepository;
        _friendshipRepository = friendshipRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<FriendRequestResponse>> Handle(AcceptFriendRequestRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var friendRequest = await _friendRequestRepository.GetByIdAsync(request.RequestId, cancellationToken);

        if (friendRequest is null)
            return Error.NotFound("FriendRequest.NotFound", "Solicitação de amizade não encontrada.");

        if (friendRequest.ToUserId != userId)
            return Error.Forbidden("FriendRequest.Forbidden", "Usuário não autorizado a responder essa solicitação.");

        if (friendRequest.Status != Domain.Enums.FriendRequestStatus.Pending)
            return Error.Conflict("FriendRequest.AlreadyResponded", "Solicitação já respondida.");

        friendRequest.Accept();

        _friendRequestRepository.Update(friendRequest);

        var friendship = Friendship.Create(friendRequest.FromUserId, friendRequest.ToUserId);

        await _friendshipRepository.AddAsync(friendship, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new FriendRequestResponse(friendRequest.Status);
    }
}
