using Dialogo.Application.Features.Friend.Shared;
using Dialogo.Domain.Shared.Interfaces;
using Dialogo.Domain.Shared.Results;

namespace Dialogo.Application.Features.Friend.RejectFriendRequest;

public class RejectFriendRequestHandler
{
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectFriendRequestHandler(IFriendRequestRepository friendRequestRepository, IUnitOfWork unitOfWork)
    {
        _friendRequestRepository = friendRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FriendRequestResponse>> Handle(RejectFriendRequestRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var friendRequest = await _friendRequestRepository.GetByIdAsync(request.RequestId, cancellationToken);

        if (friendRequest is null)
            return Error.NotFound("FriendRequest.NotFound", "Solicitação de amizade não encontrada.");

        if (friendRequest.ToUserId != userId)
            return Error.Forbidden("FriendRequest.Forbidden", "Usuário não autorizado a responder essa solicitação.");

        if (friendRequest.Status != Domain.Enums.FriendRequestStatus.Pending)
            return Error.Conflict("FriendRequest.AlreadyResponded", "Solicitação já respondida.");

        friendRequest.Reject();

        _friendRequestRepository.Update(friendRequest);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new FriendRequestResponse(friendRequest.Status);
    }
}
