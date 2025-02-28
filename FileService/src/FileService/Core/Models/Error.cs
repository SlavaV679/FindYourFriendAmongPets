namespace FileService.Core.Models;

public record Error
{
    private const string SEPARATOR = "||";
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);

    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);

    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);
    
    public static Error Null(string errorCode, string errorMessage) =>
        new(errorCode, errorMessage, ErrorType.Null);

    public string Serialize() => string.Join(SEPARATOR, Code, Message, Type);

    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split(SEPARATOR);
        if (parts.Length < 2)
            throw new InvalidOperationException("invalid serialize format");

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
            throw new InvalidOperationException("invalid serialize format");

        return new Error(parts[0], parts[1], type);
    }

    public ErrorList ToErrorList() => new ErrorList([this]);
}