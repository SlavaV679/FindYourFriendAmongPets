namespace FindYourFriendAmongPets.Application.FileProvider;

public record FileData(IEnumerable<FileContent> Files, string BucketName);