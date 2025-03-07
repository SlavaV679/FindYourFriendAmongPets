﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects.Ids;
using PetFriend.Volunteers.Domain.SpeciesAggregate;

namespace PetFriend.Volunteers.Infrastructure.Configurations.Write;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");

        // builder.HasKey(s => s.IdDelete);
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(id => id.Value,
                value => SpeciesId.Create(value));

        builder.Property(s => s.Name)
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
            .IsRequired();

        builder.OwnsMany(s => s.Breeds,
            breed =>
            {
        breed.Property(br => br.Id)
            .HasConversion(id => id.Value,
                value => BreedId.Create(value));
        
            breed.Property(br => br.Name)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .IsRequired();
        });
    }
}