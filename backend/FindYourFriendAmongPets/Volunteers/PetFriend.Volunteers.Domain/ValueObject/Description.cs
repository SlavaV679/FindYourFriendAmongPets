using CSharpFunctionalExtensions;
using PetFriend.SharedKernel;

namespace PetFriend.Volunteers.Domain.ValueObject;

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