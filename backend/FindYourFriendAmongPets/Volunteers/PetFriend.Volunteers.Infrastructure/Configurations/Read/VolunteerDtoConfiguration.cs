using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFriend.Core.Dtos;

namespace PetFriend.Volunteers.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);

        // builder.HasMany(v => v.SocialNetworksDto)
        //     .WithOne()
        //     .HasForeignKey(s => s.VolunteerId);
        //
        // builder.HasMany(v => v.RequisitesForHelpDto)
        //     .WithOne()
        //     .HasForeignKey(r => r.VolunteerId);

        // builder.HasMany(v => v.Pets)
        //     .WithOne()
        //     .HasForeignKey(p => p.VolunteerId);
    }
}