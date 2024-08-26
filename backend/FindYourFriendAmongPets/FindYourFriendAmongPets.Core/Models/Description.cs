using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Core.Models;

public record Description
{
    private Description(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_DESCRIPTION_LENGHT)
            return Errors.General.ValueIsInvalid("Description");

        return new Description(value);
    }
}