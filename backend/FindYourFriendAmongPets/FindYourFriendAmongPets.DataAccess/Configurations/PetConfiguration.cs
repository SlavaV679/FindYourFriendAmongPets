using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Models.SpeciesAggregate;
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

        builder.ComplexProperty(p => p.Description, val =>
        {
            val.Property(v => v.Value)
                .HasMaxLength(Constants.MAX_DESCRIPTION_LENGHT)
                .IsRequired()
                .HasColumnName("description");
        });

        builder.Property(p => p.Color)
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
            .IsRequired();

        builder.ComplexProperty(p => p.PetSpecies, ps =>
        {
            ps.Property(p => p.SpeciesId)
                .HasConversion(id => id.Value,
                    value => SpeciesId.Create(value))
                .HasColumnName("pet_species");
        });

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

        builder.ComplexProperty(p => p.OwnersPhoneNumber, phone =>
        {
            phone.Property(p => p.Number)
                .HasMaxLength(Constants.MAX_PHONENUMBER_LENGHT)
                .HasColumnName("owners_phone_number");
        });

        builder.Property(p => p.HelpStatus)
            .HasConversion(
                v => v.ToString(),
                v => (Status)Enum.Parse(typeof(Status), v));

        builder.OwnsOne(d => d.PetPhotos, photoBuilder =>
        {
            photoBuilder.ToJson("pet_photos");

            photoBuilder.OwnsMany(phl => phl.Values, photoListBuilder =>
            {
                photoListBuilder.Property(pp => pp.PathToStorage)
                    .HasConversion(
                        p => p.Path,
                        value => FilePath.Create(value).Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_PATH_TO_STORAGE_LENGHT);
            });
        });

        builder.OwnsOne(p => p.RequisiteDetails, petBuilder =>
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

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}