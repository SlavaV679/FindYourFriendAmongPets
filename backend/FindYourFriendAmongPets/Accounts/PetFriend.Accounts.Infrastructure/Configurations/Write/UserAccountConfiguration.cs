using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFriend.Accounts.Domain;
using PetFriend.SharedKernel;

namespace PetFriend.Accounts.Infrastructure.Configurations.Write;

public class UserAccountConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // builder.HasOne(u => u.Role)
        //     .WithMany(r => r.Users)
        //     .HasForeignKey(u => u.RoleId)
        //     .OnDelete(DeleteBehavior.Cascade);
        
        builder.ComplexProperty(u => u.FullName, fullName =>
        {
            fullName.Property(f => f.FirstName)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("first_name");
            fullName.Property(f => f.LastName)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("last_name");
            fullName.Property(f => f.Patronymic)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("patronymic");
        });
    }
}