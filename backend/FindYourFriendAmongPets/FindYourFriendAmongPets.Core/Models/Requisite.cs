using CSharpFunctionalExtensions;

namespace FindYourFriendAmongPets.Core.Models;

public record Requisite
{  
    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    public static Result<Requisite> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Requisite>($"{nameof(Name)} can not be empty.");

        return Result.Success(new Requisite(name, description));
    }
}