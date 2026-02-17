using System.ComponentModel.DataAnnotations;

namespace Dialogo.Application.Features.Friend.FriendRequest;

public record SendFriendRequestRequest([Required] string PublicCode);
