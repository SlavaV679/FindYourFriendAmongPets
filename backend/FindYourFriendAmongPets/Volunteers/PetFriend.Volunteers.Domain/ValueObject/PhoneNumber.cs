﻿using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFriend.SharedKernel;

namespace PetFriend.Volunteers.Domain.ValueObject;

public record PhoneNumber
{
    private PhoneNumber(string number)
    {
        Number = number;
    }
    
    public string Number { get; }
    
    public static Result<PhoneNumber, Error> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number) || !ValidationRegex.IsMatch(number))
            return Errors.General.ValueIsInvalid("PhoneNumber");

        return new PhoneNumber(number);
    }
    
    public static readonly Regex ValidationRegex = new Regex(
        @"(^\+\d{1,3}\d{10}$|^$)",
        RegexOptions.Singleline | RegexOptions.Compiled);
}
