using PetFriend.SharedKernel.ValueObjects;

namespace PetFriend.Core.Providers;

public record FileData(Stream Stream, FileInfo Info);
public record FileInfo(FilePath FilePath, string BucketName);