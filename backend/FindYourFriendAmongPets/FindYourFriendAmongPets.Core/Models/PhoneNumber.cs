using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace FindYourFriendAmongPets.Core.Models;

public record PhoneNumber
{
    private PhoneNumber(string number)
    {
        Number = number;
    }
    
    public string Number { get; }
    
    public static Result<PhoneNumber> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number) || !ValidationRegex.IsMatch(number))
            return Result.Failure<PhoneNumber>($"{nameof(number)} incorrect format");

        return Result.Success(new PhoneNumber(number));
    }
    
    public static readonly Regex ValidationRegex = new Regex(
        @"(^\+\d{1,3}\d{10}$|^$)",
        RegexOptions.Singleline | RegexOptions.Compiled);
}
