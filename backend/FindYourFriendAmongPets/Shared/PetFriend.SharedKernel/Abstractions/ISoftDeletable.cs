namespace PetFriend.SharedKernel.Abstractions;

public interface ISoftDeletable
{
    void Delete();

    void Restore();
}