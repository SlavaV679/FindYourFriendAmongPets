﻿using CSharpFunctionalExtensions;

namespace PetFriend.SharedKernel.ValueObjects;

public class FilePath : CSharpFunctionalExtensions.ValueObject
{
    private FilePath(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        // валидация на доступные расширения файлов
        if (extension != ".jpg" && extension != ".png")
            return Errors.General.ValueIsInvalid("extension", "extension may be only 'jpg' or 'png'");

        var fullPath = path + extension;

        return new FilePath(fullPath);
    }

    public static Result<FilePath, Error> Create(string fullPath)
    {
        var extension = System.IO.Path.GetExtension(fullPath);

        // валидация на доступные расширения файлов
        if (extension != ".jpg" && extension != ".png")
            return Errors.General.ValueIsInvalid("extension", "extension may be only 'jpg' or 'png'");

        if (string.IsNullOrWhiteSpace(fullPath))
            return Errors.General.ValueIsInvalid("file path");

        return new FilePath(fullPath);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Path;
    }
}