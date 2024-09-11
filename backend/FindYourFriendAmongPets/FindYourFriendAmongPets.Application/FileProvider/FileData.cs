using FindYourFriendAmongPets.Core.Models;

namespace FindYourFriendAmongPets.Application.FileProvider;

public record FileData(Stream Stream, FilePath FilePath, string BucketName);