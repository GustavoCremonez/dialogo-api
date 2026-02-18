using Dialogo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dialogo.Infrastructure.Persistence.Configurations;

public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
        builder.ToTable("FriendRequests");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.FromUserId, x.ToUserId })
               .IsUnique();

        builder.Property(x => x.Status)
               .IsRequired();

        builder.HasOne(x => x.FromUser)
               .WithMany(x => x.SentFriendRequests)
               .HasForeignKey(x => x.FromUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToUser)
               .WithMany(x => x.ReceivedFriendRequests)
               .HasForeignKey(x => x.ToUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
