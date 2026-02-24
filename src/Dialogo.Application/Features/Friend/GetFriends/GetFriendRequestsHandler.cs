using Dialogo.Domain.Shared.Interfaces;
using Dialogo.Domain.Shared.Results;

namespace Dialogo.Application.Features.Friend.GetFriends;

public class GetFriendRequestsHandler
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetFriendRequestsHandler(
        IFriendshipRepository friendshipRepository,
        ICurrentUserService currentUserService)
    {
        _friendshipRepository = friendshipRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetFriendsResponse>> Handle(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var friends = await _friendshipRepository.GetFriendsByUserAsync(
            userId,
            f => new FriendDto(f.FriendUserId, f.FriendUser.Name, f.FriendUser.PublicCode),
            f => new FriendDto(f.UserId, f.User.Name, f.User.PublicCode),
            cancellationToken);

        return Result<GetFriendsResponse>.Success(new GetFriendsResponse(friends));
    }
}
