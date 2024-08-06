using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindYourFriendAmongPets.DataAccess.Configurations
{
    public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItemEntity>
    {
        public void Configure(EntityTypeBuilder<ToDoItemEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(b => b.Title)
                .HasMaxLength(ToDoItem.MAX_TITLE_LENGHT)
                .IsRequired();

            builder.Property(b => b.Description)
                .IsRequired();

            builder.Property(b => b.DateCreated)
                .IsRequired();
        }
    }
}
