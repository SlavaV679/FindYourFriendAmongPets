namespace FindYourFriendAmongPets.Application.FileProvider;

public record FileData(Stream Stream, string BucketName, string Extension, string ObjectName);