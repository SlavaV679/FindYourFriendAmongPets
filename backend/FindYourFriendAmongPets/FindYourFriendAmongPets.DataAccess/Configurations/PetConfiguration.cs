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

        builder.Property(p => p.Color)
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
            .IsRequired();

        builder.Property(p => p.HealthInfo)
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
            .IsRequired();

        builder.ComplexProperty(v => v.Address, address =>
        {
            address.Property(f => f.City)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("city")
                .IsRequired();
            address.Property(f => f.Street)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("street")
                .IsRequired();
            address.Property(f => f.Building)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("building");
            address.Property(f => f.Description)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("address_description");
            address.Property(f => f.Country)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("country");
        });

        builder.Property(p => p.OwnersPhoneNumber)
            .HasMaxLength(Constants.MAX_PHONENUMBER_LENGHT)
            .IsRequired();

        builder.Property(p => p.HelpStatus)
            .HasConversion(
                v => v.ToString(),
                v => (Status)Enum.Parse(typeof(Status), v));
        
        builder.OwnsMany(d => d.PetPhotos, photoBuilder =>
        {
            photoBuilder.Property(pp => pp.Id)
                .HasConversion(id => id.Value,
                    value => PetPhotoId.Create(value));
            
            photoBuilder.Property(pp => pp.PathToStorage)
                .IsRequired()
                .HasMaxLength(Constants.MAX_PATH_TO_STORAGE_LENGHT);
        });

        builder.OwnsOne(p => p.Details, petBuilder =>
        {
            petBuilder.ToJson();

            petBuilder.OwnsMany(d => d.Requisites, requisitesBuilder =>
            {
                requisitesBuilder.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
                requisitesBuilder.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGHT);
            });
        });
    }
}