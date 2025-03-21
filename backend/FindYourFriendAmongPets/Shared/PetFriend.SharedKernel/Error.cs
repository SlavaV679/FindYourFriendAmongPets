﻿namespace PetFriend.SharedKernel;

public record Error
{
    public const string SEPARATOR = "||";
    
    public string Code { get; }
    
    public string Message { get; }
    
    public ErrorType Type { get; }
    
    public string? InvalidField { get; }

    private Error(string code, string message, ErrorType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        invalidField = invalidField;
    }

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Validation, invalidField);

    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);

    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);

    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);
    
    public string Serialize()
    {
        return string.Join(SEPARATOR, Code, Message, Type);
    }

    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split(SEPARATOR);

        if (parts.Length < 3)
        {
            // throw new ArgumentException("Invalid serialized format");
            // пока этот метод может работать только со своими ошибками.
            // Но чтобы приложение не падало временно сделал обработку ошибок из неизвестных источников следующим образом.
            parts = new string[3];
            parts[0] = "unknown.source.of.error";
            parts[1] = serialized;
            parts[2] = ErrorType.Failure.ToString();
        }

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
        {
            throw new ArgumentException("Invalid serialized format");
        }

        return new Error(parts[0], parts[1], type);
    }
    
    public ErrorList ToErrorList() => new([this]);
}