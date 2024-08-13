using CSharpFunctionalExtensions;

namespace FindYourFriendAmongPets.Core.Models;

public class FullName
{
    public const int FIRSTNAME_MAX_LENGHT = 50;
    public const int LASTNAME_MAX_LENGHT = 50;

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; }

    public string LastName { get; }

    public Result<FullName> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > FIRSTNAME_MAX_LENGHT)
            return Result.Failure<FullName>($"{nameof(FirstName)} can not be empty or bigger then {FIRSTNAME_MAX_LENGHT}");

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > LASTNAME_MAX_LENGHT)
            return Result.Failure<FullName>($"{nameof(LastName)} can not be empty or bigger then {LASTNAME_MAX_LENGHT}");

        return Result.Success(new FullName(firstName, lastName));
    }

    public override bool Equals(object? obj)
    {
        if (obj is FullName fullName
            && this.FirstName.Equals(fullName.FirstName)
            && this.LastName.Equals(fullName.LastName))
        {
            return true;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return StringComparer.Ordinal.GetHashCode($"{FirstName}{LastName}");
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}