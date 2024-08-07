using FindYourFriendAmongPets.Core.Abstractions;
using FindYourFriendAmongPets.Core.Models;

namespace FindYourFriendAmongPets.Application.Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IToDoItemsRepository _toDoItemsRepository;
        public ToDoItemService(IToDoItemsRepository toDoItemsRepository)
        {
            _toDoItemsRepository = toDoItemsRepository;
        }

        public async Task<List<ToDoItem>> GetAllToDoItems()
        {
            return await _toDoItemsRepository.Get();
        }

        public async Task<ToDoItem> GetToDoItem(Guid id)
        {
            return await _toDoItemsRepository.Get(id);
        }

        public async Task<Guid> CreateToDoItem(ToDoItem toDoItem)
        {
            return await _toDoItemsRepository.Create(toDoItem);
        }

        public async Task<Guid> UpdateToDoItem(Guid id, string title, string description)
        {
            return await _toDoItemsRepository.Update(id, title, description);
        }

        public async Task<Guid> DeleteToDoItem(Guid id)
        {
            return await _toDoItemsRepository.Delete(id);
        }
    }
}
