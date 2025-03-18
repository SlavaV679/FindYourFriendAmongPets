namespace FileService.Core.Models;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelope
{
    public object? Result { get; }
    public ErrorList? Errors { get; }
    public DateTimeOffset CreatedAt { get; }

    private Envelope(object? result, ErrorList? errors)
    {
        Result = result;
        Errors = errors;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Envelope Ok(object? result) => new(result, null);

    public static Envelope Error(ErrorList errors ) => new(null, errors);
}