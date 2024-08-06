using FindYourFriendAmongPets.Core.Models;

namespace FindYourFriendAmongPets.Core.Abstractions
{
    public interface IToDoItemRepository
    {
        Task<Guid> Create(ToDoItem toDoItem);
        
        Task<Guid> Delete(Guid id);
        
        Task<List<ToDoItem>> Get();
        
        Task<ToDoItem> Get(Guid id);

        Task<Guid> Update(Guid id, string title, string description, DateTime dateCreated);
    }
}
