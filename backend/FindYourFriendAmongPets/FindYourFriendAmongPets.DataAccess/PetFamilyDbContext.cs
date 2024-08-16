using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FindYourFriendAmongPets.DataAccess;

public class PetFamilyDbContext(IConfiguration _configuration) : DbContext
{
    private const string PET_FAMILY_DATABASE = "PetFamilyDb";

    //TODO разобраться почему с этим конструктором не работает
    // public readonly IConfiguration _configuration;
    //
    // public PetFamilyDbContext(DbContextOptions<PetFamilyDbContext> options, IConfiguration configuration) : base(options)
    // {
    //     _configuration = configuration;
    // }

    //public DbSet<Volunteer> Volunteers { get; set; } // чем отличается эти два вида объявления?
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(PET_FAMILY_DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        base.OnConfiguring(optionsBuilder);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VolunteerConfiguration());
        modelBuilder.ApplyConfiguration(new PetConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}