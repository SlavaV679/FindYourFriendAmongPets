using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFriend.Accounts.Domain.TypeAccounts;

namespace PetFriend.Accounts.Infrastructure.Configurations.Write;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");
        builder.HasKey(b => b.Id);
    }
}