using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFriend.Accounts.Domain;

namespace PetFriend.Accounts.Infrastructure.Configurations.Write;

public class RefreshSessionConfiguration : IEntityTypeConfiguration<RefreshSession>
{
    public void Configure(EntityTypeBuilder<RefreshSession> builder)
    {
        builder.ToTable("refresh_sessions");

        builder.HasKey(r => r.Id);

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);
    }
}