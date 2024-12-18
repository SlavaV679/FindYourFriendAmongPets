﻿using CSharpFunctionalExtensions;

namespace PetFriend.SharedKernel.ValueObjects.Ids;

public class VolunteerId : ValueObject
{
    public VolunteerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static VolunteerId Create(Guid value) => new(value);

    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());

    public static VolunteerId Empty() => new(Guid.Empty);
   
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}