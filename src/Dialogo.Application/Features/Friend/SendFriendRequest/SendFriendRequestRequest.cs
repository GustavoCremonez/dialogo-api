using System.ComponentModel.DataAnnotations;

namespace Dialogo.Application.Features.Friend.SendFriendRequest;

public record SendFriendRequestRequest([Required] string PublicCode);
