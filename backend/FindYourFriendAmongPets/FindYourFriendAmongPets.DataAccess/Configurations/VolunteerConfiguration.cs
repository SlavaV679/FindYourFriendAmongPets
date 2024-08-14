using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindYourFriendAmongPets.DataAccess.Configurations;

public class VolunteerConfiguration:IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("Volunteer");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_DESCRIPTION_LENGHT);

        builder.Property(v => v.PhoneNumber)
            .HasMaxLength(Constants.MAX_PHONENUMBER_LENGHT);

        builder.HasMany(v => v.SocialNetworks)
            .WithOne()
            .HasForeignKey("social_network_id");
        
        builder.HasMany(v => v.RequisitesForHelp)
            .WithOne()
            .HasForeignKey("requisite_for_help_id");

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("pet_id");
    }
}