﻿using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindYourFriendAmongPets.DataAccess.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);

        builder.ComplexProperty(v => v.FullName, fullName =>
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
        
        builder.Property(v => v.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_DESCRIPTION_LENGHT);

        builder.Property(v => v.PhoneNumber)
            .HasMaxLength(Constants.MAX_PHONENUMBER_LENGHT);

        // builder.HasMany(v => v.SocialNetworks)
        //     .WithOne()
        //     .HasForeignKey("volunteer_id");//такой способ более понятный чем OwnsMany, однако
            // здесь не смог настроить длину внутренних полей, например SocialNetworks.Link

        builder.OwnsMany(
            v => v.SocialNetworks,
            s =>
            {
                s.Property(x => x.Link).HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
                s.Property(x => x.Title).HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
            }); //TODO научиться управлять onDelete: ReferentialAction.Cascade
        
        builder.HasMany(v => v.RequisitesForHelp)
            .WithOne()
            .HasForeignKey("volunteer_id");

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id");
    }
}

// в этой конфигурации VolunteerConfiguration реализованы "три вида" свойств:
// 1. FullName (ComplexProperty) как настоящий Value Object, не имеет id, иммутабельный за счет определения как record,
// и тут доавлен через ComplexProperty
// 2. RequisitesForHelp (HasMany().WithOne()) - стандартная реализация через зависимую таблицу. Однако в этом случае не смог настроить
// внутренние свойства, т.е. например длину RequisitesForHelp.Name. Для решения этой проблемы использовал реализацию
// OwnsMany для свойства SocialNetworks. RequisitesForHelp реализована через HasMany().WithOne()
// 3. SocialNetworks (OwnsMany) - реализация через OwnsMany.
// В PetConfiguration.Details реализован 4-ый вид подобного свойства через ToJson()!!!
// Этот вид проектирования создает колонку, в которой данные будут храниться в виде json.

// Нужно иметь ввиду что при проектировании микросервисной архитектуры агрегаты из разных доменов
// не могут ссылаться друг на друга через идентификатор. ** надо уточнить правильно ли это.