namespace PetFriend.Core.Abstractions;

public interface IFilesCleanerService
{
    Task Process(CancellationToken cancellationToken);
}