using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFriend.SharedKernel;
using PetFriend.Volunteers.Domain;
using PetFriend.Volunteers.Domain.SpeciesAggregate;
using PetFriend.Volunteers.Infrastructure.Interceptors;

namespace PetFriend.Volunteers.Infrastructure.DbContexts;

public class PetFamilyWriteDbContext(IConfiguration _configuration) : DbContext
{
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
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(Constants.PET_FAMILY_DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    private ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(builder => { builder.AddConsole(); });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PetFamilyWriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
        // modelBuilder.ApplyConfiguration(new VolunteerConfiguration());
        // modelBuilder.ApplyConfiguration(new PetConfiguration());
        // modelBuilder.ApplyConfiguration(new SpeciesConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}