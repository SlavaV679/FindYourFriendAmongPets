﻿using CSharpFunctionalExtensions;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.Abstractions;
using PetFriend.SharedKernel.ValueObjects;
using PetFriend.SharedKernel.ValueObjects.Ids;
using PetFriend.Volunteers.Domain.Enums;
using PetFriend.Volunteers.Domain.ValueObject;

namespace PetFriend.Volunteers.Domain.Entities;

public class Pet : SharedKernel.Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;

    private readonly List<PetFile> _petFiles = [];

    private Pet(PetId id) : base(id)
    {
    }

    private Pet(PetId id,
        string name,
        PetSpecies petSpecies,
        Description description,
        string color,
        string healthInfo,
        Address address,
        double weight,
        double height,
        PhoneNumber ownersPhoneNumber,
        bool isNeutered,
        DateTime dateOfBirth,
        bool isVaccinated,
        Status helpStatus,
        DateTime dateCreated,
        PetRequisiteDetails requisiteDetails,
        ValueObjectList<PetFile>? petFiles)
        : base(id)
    {
        Name = name;
        PetSpecies = petSpecies;
        Description = description;
        Color = color;
        HealthInfo = healthInfo;
        Address = address;
        Weight = weight;
        Height = height;
        OwnersPhoneNumber = ownersPhoneNumber;
        IsNeutered = isNeutered;
        DateOfBirth = dateOfBirth;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        DateCreated = dateCreated;
        RequisiteDetails = requisiteDetails;
        PetFiles = petFiles ?? new ValueObjectList<PetFile>([]);
    }

    public string Name { get; private set; }

    public PetSpecies PetSpecies { get; private set; }

    public Description Description { get; private set; }

    public string Color { get; private set; }

    public string HealthInfo { get; private set; }

    public Address Address { get; private set; }

    public double Weight { get; private set; }

    public double Height { get; private set; }

    public PhoneNumber OwnersPhoneNumber { get; private set; }

    public bool IsNeutered { get; private set; }

    public DateTime DateOfBirth { get; private set; }

    public bool IsVaccinated { get; private set; }

    public Status HelpStatus { get; private set; }

    public DateTime DateCreated { get; private set; }
    
    public Position Position { get; private set; }

    public IReadOnlyList<PetFile> PetFiles { get; private set; }

    public PetRequisiteDetails RequisiteDetails { get; private set; }

    public static Result<Pet, Error> Create(PetId id,
        string name,
        PetSpecies petSpecies,
        Description description,
        string color,
        string healthInfo,
        Address address,
        double weight,
        double height,
        PhoneNumber ownersPhoneNumber,
        bool isNeutered,
        DateTime dateOfBirth,
        bool isVaccinated,
        Status helpStatus,
        PetRequisiteDetails requisiteDetails,
        ValueObjectList<PetFile>? petFiles
    )
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_LOW_TEXT_LENGHT)
            return Errors.General.ValueIsInvalid(
                nameof(Name), $"{nameof(Name)} can not be empty or bigger then {Constants.MAX_LOW_TEXT_LENGHT}");

        if (string.IsNullOrWhiteSpace(color) || color.Length > Constants.MAX_LOW_TEXT_LENGHT)
            return Errors.General.ValueIsInvalid(
                nameof(Color), $"{nameof(Color)} can not be empty or bigger then {Constants.MAX_LOW_TEXT_LENGHT}");

        if (string.IsNullOrWhiteSpace(healthInfo) || healthInfo.Length > Constants.MAX_HIGH_TEXT_LENGHT)
            return Errors.General.ValueIsInvalid(
                nameof(HealthInfo),
                $"{nameof(HealthInfo)} can not be empty or bigger then {Constants.MAX_HIGH_TEXT_LENGHT}");

        return new Pet(id,
            name,
            petSpecies,
            description,
            color,
            healthInfo,
            address,
            weight,
            height,
            ownersPhoneNumber,
            isNeutered,
            dateOfBirth,
            isVaccinated,
            helpStatus,
            DateTime.Now.ToUniversalTime(),
            requisiteDetails,
            petFiles);
    }

    public void AddPetFiles(IEnumerable<PetFile> petFiles)
    {
        _petFiles.AddRange(petFiles);
    }

    public void UpdateFiles(ValueObjectList<PetFile> petFiles) => PetFiles = petFiles;

    public void Delete()
    {
        _isDeleted = true;
    }

    public void Restore()
    {
        _isDeleted = false;
    }
    
    internal void SetPosition(Position position) => Position = position;
    
    public UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    public UnitResult<Error> MoveBack()
    {
        var newPosition = Position.Back();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    public void Move(Position newPosition) =>
        Position = newPosition;
}