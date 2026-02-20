namespace Dialogo.Domain.Entities
{
    public class Friendship
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public Guid FriendUserId { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public User User { get; private set; }

        public User FriendUser { get; private set; }

        public static Friendship Create(Guid userId, Guid friendUserId)
        {
            var now = DateTime.UtcNow;

            return new Friendship
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FriendUserId = friendUserId,
                CreatedAt = now,
            };
        }
    }
}
