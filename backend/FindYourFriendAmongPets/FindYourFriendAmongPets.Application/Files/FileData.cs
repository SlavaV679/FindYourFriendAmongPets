using FindYourFriendAmongPets.Core.Models;

namespace FindYourFriendAmongPets.Application.Files;

public record FileData(Stream Stream, FileInfo Info);
public record FileInfo(FilePath FilePath, string BucketName);