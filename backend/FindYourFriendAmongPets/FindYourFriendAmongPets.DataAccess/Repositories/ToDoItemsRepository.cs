using FindYourFriendAmongPets.Core.Abstractions;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FindYourFriendAmongPets.DataAccess.Repositories
{
    public class ToDoItemsRepository : IToDoItemsRepository
    {
        private readonly ToDoItemDbContext _context;

        public ToDoItemsRepository(ToDoItemDbContext context)
        {
            _context = context;
        }

        public async Task<List<ToDoItem>> Get()
        {
            var toDoItemEntities = await _context.ToDoItems
                .AsNoTracking()
                .ToListAsync();

            var toDoItems = toDoItemEntities
                .Select(x => ToDoItem.Create(x.Id, x.Title, x.Description, x.DateCreated).ToDoItem)
                .ToList();

            return toDoItems;
        }

        public async Task<ToDoItem> Get(Guid id)
        {
            var toDoItemEntity = await _context.ToDoItems
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (toDoItemEntity == null)
            {
                //ToDoItem.Create().Error
                throw new Exception($"ToDoItem, where Id = '{id}' not fonded.");
            }
            var toDoItem = ToDoItem.Create(toDoItemEntity.Id, toDoItemEntity.Title, toDoItemEntity.Description, toDoItemEntity.DateCreated).ToDoItem;

            return toDoItem;
        }

        public async Task<Guid> Create(ToDoItem toDoItem)
        {
            var toDoItemEntity = new ToDoItemEntity
            {
                Id = toDoItem.Id,
                Title = toDoItem.Title,
                Description = toDoItem.Description,
                DateCreated = toDoItem.DateCreated,
            };

            await _context.ToDoItems.AddAsync(toDoItemEntity);
            await _context.SaveChangesAsync();

            return toDoItemEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string title, string description, DateTime dateCreated)
        {
            await _context.ToDoItems
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(i => i.Title, i => title)
                    .SetProperty(i => i.Description, i => description)
                    .SetProperty(i => i.DateCreated, dateCreated)
                );

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.ToDoItems
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }
    }
}
