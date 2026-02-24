using Dialogo.Domain.Shared.Interfaces;
using Dialogo.Domain.Shared.Results;

namespace Dialogo.Application.Features.Friend.GetFriends;

public class GetFriendRequestsHandler
{
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetFriendRequestsHandler(
        IFriendRequestRepository friendRequestRepository,
        ICurrentUserService currentUserService)
    {
        _friendRequestRepository = friendRequestRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetFriendsResponse>> Handle(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var friends = await _friendRequestRepository.GetByUserProjectionAsync(
            userId,
            fr => new FriendDto(fr.ToUserId, fr.ToUser.Name, fr.ToUser.PublicCode),
            cancellationToken);

        return Result<GetFriendsResponse>.Success(new GetFriendsResponse(friends));
    }
}
