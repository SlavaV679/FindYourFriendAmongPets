using FindYourFriendAmongPets.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FindYourFriendAmongPets.DataAccess
{
    public class ToDoItemDbContext : DbContext
    {
        public ToDoItemDbContext(DbContextOptions<ToDoItemDbContext> options) : base(options) { }
    
        public DbSet<ToDoItemEntity> ToDoItems { get; set; }
    }
}
