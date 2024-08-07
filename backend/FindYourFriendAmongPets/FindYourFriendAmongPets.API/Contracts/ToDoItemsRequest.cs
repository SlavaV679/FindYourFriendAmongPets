namespace FindYourFriendAmongPets.API.Contracts
{
    public record ToDoItemsRequest(
        string Title,
        string Description,
        DateTime DateCreated);
}