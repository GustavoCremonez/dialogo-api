using System.ComponentModel.DataAnnotations;

namespace Dialogo.Application.Features.Friend.RejectFriendRequest;

public record RejectFriendRequestRequest([Required] Guid RequestId);
