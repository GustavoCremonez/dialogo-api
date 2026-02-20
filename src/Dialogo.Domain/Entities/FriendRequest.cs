using Dialogo.Domain.Enums;

namespace Dialogo.Domain.Entities;

public class FriendRequest
{
    public Guid Id { get; private set; }

    public Guid FromUserId { get; private set; }

    public Guid ToUserId { get; private set; }

    public FriendRequestStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? RespondedAt { get; private set; }

    public User FromUser { get; private set; }

    public User ToUser { get; private set; }

    private FriendRequest() { }

    public static FriendRequest Create(Guid fromUserId, Guid toUserId)
    {
        var now = DateTime.UtcNow;

        return new FriendRequest
        {
            Id = Guid.NewGuid(),
            FromUserId = fromUserId,
            ToUserId = toUserId,
            Status = FriendRequestStatus.Pending,
            CreatedAt = now,
        };
    }

    public void Accept()
    {
        Status = FriendRequestStatus.Accepted;
        RespondedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        Status = FriendRequestStatus.Rejected;
        RespondedAt = DateTime.UtcNow;
    }
}
