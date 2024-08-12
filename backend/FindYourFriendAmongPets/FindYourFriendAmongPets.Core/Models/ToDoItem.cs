namespace FindYourFriendAmongPets.Core.Models
{
    public class ToDoItem
    {
        public const int MAX_TITLE_LENGHT = 150;

        private ToDoItem(Guid id, string title, string description, DateTime dateCreated)
        {
            Id = id;
            Title = title; 
            Description = description; 
            DateCreated = dateCreated;
        }

        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime DateCreated { get; }

        public static (ToDoItem ToDoItem, string Error) Create(Guid id, string title, string description, DateTime dateCreated)
        {
            var error = String.Empty;

            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGHT)
            {
                error = $"Title not be able empty or longer then {MAX_TITLE_LENGHT} symbols";
            }

            var toDoItem = new ToDoItem(id, title, description, dateCreated);

            return (toDoItem, error);
        }
    }
}
