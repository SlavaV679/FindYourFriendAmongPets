using FindYourFriendAmongPets.Core.Models;

namespace FindYourFriendAmongPets.Core.Abstractions
{
    public interface IToDoItemService
    {
        Task<Guid> CreateToDoItem(ToDoItem toDoItem);

        Task<Guid> DeleteToDoItem(Guid id);

        Task<List<ToDoItem>> GetAllToDoItems();

        Task<ToDoItem> GetToDoItem(Guid id);

        Task<Guid> UpdateToDoItem(Guid id, string title, string description, DateTime dateCreated);
    }
}
