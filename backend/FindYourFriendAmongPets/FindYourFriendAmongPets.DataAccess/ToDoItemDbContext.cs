using FindYourFriendAmongPets.DataAccess.Configurations;
using FindYourFriendAmongPets.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FindYourFriendAmongPets.DataAccess
{
    public class ToDoItemDbContext : DbContext
    {
        public ToDoItemDbContext(DbContextOptions<ToDoItemDbContext> options) : base(options) { }
    
        public DbSet<ToDoItemEntity> ToDoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ToDoItemConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
