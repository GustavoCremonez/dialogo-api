namespace Dialogo.Application.Features.Friend.GetFriends;

public record FriendDto(Guid Id, string Name, string PublicCode);

public record GetFriendsResponse(IEnumerable<FriendDto> Friends);
