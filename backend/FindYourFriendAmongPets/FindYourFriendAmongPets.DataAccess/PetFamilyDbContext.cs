using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Models.SpeciesAggregate;
using FindYourFriendAmongPets.DataAccess.Configurations;
using FindYourFriendAmongPets.DataAccess.Interceptors;
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

    public DbSet<Species> Species => Set<Species>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(PET_FAMILY_DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VolunteerConfiguration());
        modelBuilder.ApplyConfiguration(new PetConfiguration());
        modelBuilder.ApplyConfiguration(new SpeciesConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}