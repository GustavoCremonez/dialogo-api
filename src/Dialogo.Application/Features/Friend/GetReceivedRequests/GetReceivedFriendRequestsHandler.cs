using Dialogo.Domain.Shared.Interfaces;
using Dialogo.Domain.Shared.Results;

namespace Dialogo.Application.Features.Friend.GetReceivedRequests;

public class GetReceivedFriendRequestsHandler
{
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetReceivedFriendRequestsHandler(
        IFriendRequestRepository friendRequestRepository,
        ICurrentUserService currentUserService)
    {
        _friendRequestRepository = friendRequestRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetReceivedFriendRequestsResponse>> Handle(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var requests = await _friendRequestRepository.GetReceivedPendingProjectionAsync(
            userId,
            fr => new ReceivedFriendRequestDto(fr.Id, fr.FromUser.Name, fr.FromUser.PublicCode),
            cancellationToken);

        return Result<GetReceivedFriendRequestsResponse>.Success(new GetReceivedFriendRequestsResponse(requests));
    }
}
