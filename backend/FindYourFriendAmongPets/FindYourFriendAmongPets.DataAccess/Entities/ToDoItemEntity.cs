namespace FindYourFriendAmongPets.DataAccess.Entities
{
    public class ToDoItemEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
