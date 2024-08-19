using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Core.Models;

public record Address
{
    private Address(string city, string street, string? building = null, string? description = null, string? country = null)
    {
        City = city;
        Street = street;
        Building = building;
        Description = description;
        Country = country;
    }

    public string? Country { get; }

    public string City { get; }

    public string Street { get; }

    public string? Building { get; }

    public string? Description { get; }

    public static Result<Address> Create(string city, string street, string? building = null, string? description = null, string? country = null)
    {
        if (string.IsNullOrWhiteSpace(city) || city.Length > Constants.MAX_LOW_TEXT_LENGHT)
            return Result.Failure<Address>($"{nameof(City)} can not be empty or bigger then {Constants.MAX_LOW_TEXT_LENGHT}");

        if (string.IsNullOrWhiteSpace(street) || street.Length > Constants.MAX_LOW_TEXT_LENGHT)
            return Result.Failure<Address>($"{nameof(Street)} can not be empty or bigger then {Constants.MAX_LOW_TEXT_LENGHT}");

        if (building?.Length == 0 || building?.Length > Constants.MAX_LOW_TEXT_LENGHT)
            return Result.Failure<Address>($"{nameof(Building)} can not be empty or bigger then {Constants.MAX_LOW_TEXT_LENGHT}");

        if (string.IsNullOrWhiteSpace(country))
            country = "Russia";

        return Result.Success(new Address(city, street, building, description, country));
    }
}