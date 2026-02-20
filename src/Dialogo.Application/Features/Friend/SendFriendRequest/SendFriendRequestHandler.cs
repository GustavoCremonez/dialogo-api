using Dialogo.Application.Features.Friend.Shared;
using Dialogo.Domain.Entities;
using Dialogo.Domain.Enums;
using Dialogo.Domain.Shared.Interfaces;
using Dialogo.Domain.Shared.Results;

namespace Dialogo.Application.Features.Friend.SendFriendRequest;

public class SendFriendRequestHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendFriendRequestHandler(IUserRepository userRepository, IFriendRequestRepository friendRequestRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _friendRequestRepository = friendRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FriendRequestResponse>> Handle(SendFriendRequestRequest sendFriendRequestRequest, Guid fromUserId, CancellationToken cancellationToken)
    {
        var toUser = await _userRepository.GetByPublicCodeAsync(sendFriendRequestRequest.PublicCode, cancellationToken);

        if (toUser is null)
        {
            return Error.NotFound("FriendRequest.ToUserNotExist", "Usuário que deseja mandar solicitação não existe.");
        }

        var friendRequestExists = await _friendRequestRepository.ExistsAsync(
            fr => fr.FromUserId == fromUserId && fr.ToUserId == toUser.Id && fr.Status == FriendRequestStatus.Pending,
            cancellationToken);

        if (friendRequestExists)
            return Error.Conflict("FriendRequest.AlreadyExists", "Já existe uma solicitação de amizade entre esses usuários.");

        var friendRequest = FriendRequest.Create(fromUserId, toUser.Id!);

        await _friendRequestRepository.AddAsync(friendRequest, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new FriendRequestResponse(friendRequest.Status);
    }
}
