using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FindYourFriendAmongPets.DataAccess;

public class PetFamilyDbContext : DbContext
{
    private const string PET_FAMILY_DATABASE = "PetFamilyDb";
    public readonly IConfiguration _Configuration;
    public PetFamilyDbContext(DbContextOptions<PetFamilyDbContext> options, IConfiguration configuration) : base(options)
    {
        _Configuration = configuration;
    }

    public DbSet<Volunteer> Volunteers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_Configuration.GetConnectionString(PET_FAMILY_DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        //base.OnConfiguring(optionsBuilder);
    }

    private ILoggerFactory CreateLoggerFactory()=>
        LoggerFactory.Create(builder => { builder.AddConsole(); });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VolunteerConfiguration());
        modelBuilder.ApplyConfiguration(new PetConfiguration());

        //base.OnModelCreating(modelBuilder);
    }
}