using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFriend.Accounts.Domain;
using PetFriend.Accounts.Domain.TypeAccounts;
using PetFriend.SharedKernel;

namespace PetFriend.Accounts.Infrastructure.DbContexts;

public class AccountsWriteDbContext(IConfiguration _configuration) : IdentityDbContext<User, Role, Guid>
{
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<AdminAccount> Admins => Set<AdminAccount>();
    public DbSet<ParticipantAccount> Participants => Set<ParticipantAccount>();
    public DbSet<VolunteerAccount> Volunteers => Set<VolunteerAccount>();
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(Constants.PET_FAMILY_DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        base.OnConfiguring(optionsBuilder);
    }
    
    private ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(builder => { builder.AddConsole(); });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("accounts");
        
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AccountsWriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
        
        modelBuilder.Entity<User>()
            .ToTable("users");

        modelBuilder.Entity<Role>()
            .ToTable("roles");

        modelBuilder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");

        modelBuilder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");

        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");

        modelBuilder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");
    }
}
