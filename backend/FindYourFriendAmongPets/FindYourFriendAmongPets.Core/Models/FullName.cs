using System.ComponentModel.DataAnnotations.Schema;
using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Core.Models;

public record FullName
{
    public const int FIRSTNAME_MAX_LENGHT = 50;
    public const int LASTNAME_MAX_LENGHT = 50;
    public const int PATRONYMIC_MAX_LENGHT = 50;

    private FullName(string firstName, string lastName, string? patronymic = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
    }

    public string FirstName { get; }

    public string LastName { get; }

    public string? Patronymic { get; }

    public static Result<FullName, Error> Create(string firstName, string lastName, string? patronymic = null)
    {
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > FIRSTNAME_MAX_LENGHT)
            return Errors.General.ValueIsInvalid(nameof(FirstName),$"{nameof(FirstName)} can not be empty or bigger then {FIRSTNAME_MAX_LENGHT}");

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > LASTNAME_MAX_LENGHT)
            return Errors.General.ValueIsInvalid(nameof(LastName),$"{nameof(LastName)} can not be empty or bigger then {LASTNAME_MAX_LENGHT}");

        if (patronymic?.Length is 0 or > PATRONYMIC_MAX_LENGHT)
            return Errors.General.ValueIsInvalid(nameof(Patronymic),$"{nameof(Patronymic)} can not be empty or bigger then {PATRONYMIC_MAX_LENGHT}");

        return new FullName(firstName, lastName, patronymic);
    }
}

// NOTE методы Equals, GetHashCode нужно переопределять, если мы создаем Value Object через class.
// В случае record эти методы уже переопределены!!!

// public override bool Equals(object? obj)
    // {
    //     if (obj is FullName fullName
    //         && this.FirstName.Equals(fullName.FirstName)
    //         && this.LastName.Equals(fullName.LastName))
    //     {
    //         return true;
    //     }
    //
    //     return false;
    // }

    // public override int GetHashCode()
    // {
    //     return StringComparer.Ordinal.GetHashCode($"{FirstName}{LastName}");
    // }
    //
    // public override string ToString()
    // {
    //     return $"{LastName} {FirstName} {Patronymic ?? string.Empty}";
    // }
