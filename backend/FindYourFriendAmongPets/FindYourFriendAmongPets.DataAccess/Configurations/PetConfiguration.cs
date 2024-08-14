using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindYourFriendAmongPets.DataAccess.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value,
                value => PetId.Create(value));

        builder.Property(p => p.Description)
            .HasMaxLength(Constants.MAX_DESCRIPTION_LENGHT)
            .IsRequired();
        
        builder.Property(p=>p.HelpStatus)
            .HasConversion(
                v => v.ToString(),
                v => (Status)Enum.Parse(typeof(Status), v));

        builder.HasMany(p => p.Requisites)
            .WithOne()
            .HasForeignKey("requisite_id");

        builder.OwnsOne(p => p.Details, petBuilder =>
        {
            petBuilder.ToJson();
            petBuilder.OwnsMany(d => d.PetPhotos, photoBuilder =>
            {
                photoBuilder.Property(pp => pp.PathToStorage)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_PATH_TO_STORAGE_LENGHT);
            });
        });
    }
}