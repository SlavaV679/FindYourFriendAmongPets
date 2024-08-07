namespace FindYourFriendAmongPets.API.Contracts
{
    public record ToDoItemsResponse(
        Guid Id,
        string Title,
        string Description,
        DateTime DateCreated);
}