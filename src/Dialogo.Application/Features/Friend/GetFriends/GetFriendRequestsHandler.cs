using Dialogo.Domain.Entities;
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

        var friendRequests = await _friendRequestRepository.GetFriendRequestByUserAsync(userId, cancellationToken);

        var friends = MapFriend(friendRequests);

        return Result<GetFriendsResponse>.Success(new GetFriendsResponse(friends));
    }

    private List<FriendDto> MapFriend(IEnumerable<FriendRequest> friendRequests)
    {
        return friendRequests
            .Select(fr =>
            {
                var user = fr.ToUser;
                return new FriendDto(
                    fr.ToUserId,
                    user?.Name ?? string.Empty,
                    user?.PublicCode ?? string.Empty);
            })
            .ToList();
    }
}
