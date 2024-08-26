using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Shared;

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

    public static Result<Requisite, Error> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid(nameof(Name));

        return new Requisite(name, description);
    }
}