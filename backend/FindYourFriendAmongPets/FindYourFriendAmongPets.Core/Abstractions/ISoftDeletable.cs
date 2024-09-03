namespace FindYourFriendAmongPets.Core.Abstractions;

public interface ISoftDeletable
{
    void Delete();

    void Restore();
}