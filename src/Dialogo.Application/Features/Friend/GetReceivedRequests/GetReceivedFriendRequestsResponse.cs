namespace Dialogo.Application.Features.Friend.GetReceivedRequests;

public record ReceivedFriendRequestDto(Guid RequestId, string Name, string PublicCode);

public record GetReceivedFriendRequestsResponse(IEnumerable<ReceivedFriendRequestDto> Requests);
