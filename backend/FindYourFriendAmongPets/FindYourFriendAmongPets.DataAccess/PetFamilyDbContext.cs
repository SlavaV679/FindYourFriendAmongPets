using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FindYourFriendAmongPets.DataAccess;

public class PetFamilyDbContext : DbContext
{
    public PetFamilyDbContext(DbContextOptions<PetFamilyDbContext> options) : base(options)
    {
    }

    public DbSet<Volunteer> Volunteers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VolunteerConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}