using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFriend.Accounts.Domain;

namespace PetFriend.Accounts.Infrastructure.Configurations.Write;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Navigation(r => r.RolePermissions)
            .AutoInclude();
    }
}