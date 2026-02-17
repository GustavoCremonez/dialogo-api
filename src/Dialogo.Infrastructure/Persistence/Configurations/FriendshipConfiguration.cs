using Dialogo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dialogo.Infrastructure.Persistence.Configurations;

public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.ToTable("Friendships");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.UserId, x.FriendUserId })
               .IsUnique();

        builder.HasOne(x => x.User)
               .WithMany(x => x.Friendships)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.FriendUser)
               .WithMany()
               .HasForeignKey(x => x.FriendUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
