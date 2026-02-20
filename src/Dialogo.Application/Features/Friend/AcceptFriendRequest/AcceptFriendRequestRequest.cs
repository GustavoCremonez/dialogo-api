using System.ComponentModel.DataAnnotations;

namespace Dialogo.Application.Features.Friend.AcceptFriendRequest;

public record AcceptFriendRequestRequest([Required] Guid RequestId);
