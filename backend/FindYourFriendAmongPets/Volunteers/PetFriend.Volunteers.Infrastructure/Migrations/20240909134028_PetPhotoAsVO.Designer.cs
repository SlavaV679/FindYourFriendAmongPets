﻿// <auto-generated />
using System;
using System.Collections.Generic;
using FindYourFriendAmongPets.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetFriend.Volunteers.Infrastructure.DbContexts;

#nullable disable

namespace FindYourFriendAmongPets.DataAccess.Migrations
{
    [DbContext(typeof(PetFamilyWriteDbContext))]
    [Migration("20240909134028_PetPhotoAsVO")]
    partial class PetPhotoAsVO
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FindYourFriendAmongPets.Core.Models.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("color");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("HealthInfo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("health_info");

                    b.Property<double>("Height")
                        .HasColumnType("double precision")
                        .HasColumnName("height");

                    b.Property<string>("HelpStatus")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("help_status");

                    b.Property<bool>("IsNeutered")
                        .HasColumnType("boolean")
                        .HasColumnName("is_neutered");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision")
                        .HasColumnName("weight");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "FindYourFriendAmongPets.Core.Models.Pet.Address#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Building")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("building");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("city");

                            b1.Property<string>("Country")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("country");

                            b1.Property<string>("Description")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("address_description");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("street");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Description", "FindYourFriendAmongPets.Core.Models.Pet.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("OwnersPhoneNumber", "FindYourFriendAmongPets.Core.Models.Pet.OwnersPhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(13)
                                .HasColumnType("character varying(13)")
                                .HasColumnName("owners_phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PetSpecies", "FindYourFriendAmongPets.Core.Models.Pet.PetSpecies#PetSpecies", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("SpeciesId")
                                .HasColumnType("uuid")
                                .HasColumnName("pet_species");
                        });

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("FindYourFriendAmongPets.Core.Models.RequisiteForHelp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.HasKey("Id")
                        .HasName("pk_requisite_for_help");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_requisite_for_help_volunteer_id");

                    b.ToTable("requisite_for_help", (string)null);
                });

            modelBuilder.Entity("FindYourFriendAmongPets.Core.Models.SpeciesAggregate.Species", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_species");

                    b.ToTable("species", (string)null);
                });

            modelBuilder.Entity("FindYourFriendAmongPets.Core.Models.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("ExperienceInYears")
                        .HasColumnType("integer")
                        .HasColumnName("experience_in_years");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "FindYourFriendAmongPets.Core.Models.Volunteer.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "FindYourFriendAmongPets.Core.Models.Volunteer.FullName#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("first_name");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("last_name");

                            b1.Property<string>("Patronymic")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("patronymic");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "FindYourFriendAmongPets.Core.Models.Volunteer.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(13)
                                .HasColumnType("character varying(13)")
                                .HasColumnName("phone_number");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("FindYourFriendAmongPets.Core.Models.Pet", b =>
                {
                    b.HasOne("FindYourFriendAmongPets.Core.Models.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("volunteer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");

                    b.OwnsOne("FindYourFriendAmongPets.Core.Models.PetPhotosList", "PetPhotos", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("pet_photos");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");

                            b1.OwnsMany("FindYourFriendAmongPets.Core.Models.PetPhoto", "PetPhotos", b2 =>
                                {
                                    b2.Property<Guid>("PetPhotosListPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("PathToStorage")
                                        .IsRequired()
                                        .HasMaxLength(75)
                                        .HasColumnType("character varying(75)");

                                    b2.HasKey("PetPhotosListPetId", "Id");

                                    b2.ToTable("pets");

                                    b2.ToJson("pet_photos");

                                    b2.WithOwner()
                                        .HasForeignKey("PetPhotosListPetId")
                                        .HasConstraintName("fk_pets_pets_pet_photos_list_pet_id");
                                });

                            b1.Navigation("PetPhotos");
                        });

                    b.OwnsOne("FindYourFriendAmongPets.Core.Models.PetRequisiteDetails", "RequisiteDetails", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("requisite_details");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");

                            b1.OwnsMany("FindYourFriendAmongPets.Core.Models.Requisite", "Requisites", b2 =>
                                {
                                    b2.Property<Guid>("PetRequisiteDetailsPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasMaxLength(2000)
                                        .HasColumnType("character varying(2000)");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.HasKey("PetRequisiteDetailsPetId", "Id");

                                    b2.ToTable("pets");

                                    b2.ToJson("requisite_details");

                                    b2.WithOwner()
                                        .HasForeignKey("PetRequisiteDetailsPetId")
                                        .HasConstraintName("fk_pets_pets_pet_requisite_details_pet_id");
                                });

                            b1.Navigation("Requisites");
                        });

                    b.Navigation("PetPhotos")
                        .IsRequired();

                    b.Navigation("RequisiteDetails")
                        .IsRequired();
                });

            modelBuilder.Entity("FindYourFriendAmongPets.Core.Models.RequisiteForHelp", b =>
                {
                    b.HasOne("FindYourFriendAmongPets.Core.Models.Volunteer", null)
                        .WithMany("RequisitesForHelp")
                        .HasForeignKey("volunteer_id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("fk_requisite_for_help_volunteers_volunteer_id");
                });

            modelBuilder.Entity("FindYourFriendAmongPets.Core.Models.SpeciesAggregate.Species", b =>
                {
                    b.OwnsMany("FindYourFriendAmongPets.Core.Models.SpeciesAggregate.Breed", "Breeds", b1 =>
                        {
                            b1.Property<Guid>("SpeciesId")
                                .HasColumnType("uuid")
                                .HasColumnName("species_id");

                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");

                            b1.HasKey("SpeciesId", "Id")
                                .HasName("pk_breed");

                            b1.ToTable("breed", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("SpeciesId")
                                .HasConstraintName("fk_breed_species_species_id");
                        });

                    b.Navigation("Breeds");
                });

            modelBuilder.Entity("FindYourFriendAmongPets.Core.Models.Volunteer", b =>
                {
                    b.OwnsMany("FindYourFriendAmongPets.Core.Models.SocialNetwork", "SocialNetworks", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid")
                                .HasColumnName("volunteer_id");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("Link")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("link");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("title");

                            b1.HasKey("VolunteerId", "Id")
                                .HasName("pk_social_network");

                            b1.ToTable("social_network", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_social_network_volunteers_volunteer_id");
                        });

                    b.Navigation("SocialNetworks");
                });

            modelBuilder.Entity("FindYourFriendAmongPets.Core.Models.Volunteer", b =>
                {
                    b.Navigation("Pets");

                    b.Navigation("RequisitesForHelp");
                });
#pragma warning restore 612, 618
        }
    }
}