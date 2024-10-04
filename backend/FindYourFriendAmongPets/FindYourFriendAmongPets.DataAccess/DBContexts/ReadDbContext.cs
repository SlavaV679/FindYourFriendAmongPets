using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FindYourFriendAmongPets.DataAccess.DBContexts;

public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    public DbSet<VolunteerDto> Volunteers => Set<VolunteerDto>();
    
    public DbSet<PetDto> Pets => Set<PetDto>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.PET_FAMILY_DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    
    private ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(builder => { builder.AddConsole(); });
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ReadDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }
}