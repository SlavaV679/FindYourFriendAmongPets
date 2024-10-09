using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.Abstraction;
using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.Application.Extensions;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Models.SpeciesAggregate;
using FindYourFriendAmongPets.Core.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace FindYourFriendAmongPets.Application.Volunteers.Commands.AddPet;

public class AddPetHandler: ICommandHandler<Guid, AddPetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetHandler(
        IVolunteerRepository volunteerRepository,
        IUnitOfWork unitOfWork,
        IValidator<AddPetCommand> validator,
        ILogger<AddPetHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var volunteerResult = await _volunteerRepository.GetById(
            VolunteerId.Create(command.VolunteerId), cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var pet = InitPet(command);
        volunteerResult.Value.AddPet(pet);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet added to volunteer. PetId - {id}", pet.Id.Value);
        
        return pet.Id.Value;
    }

    private Pet InitPet(AddPetCommand command)
    {
        var description = Description.Create(command.Description).Value;
        
        var petSpecies = PetSpecies
            .Create(SpeciesId.Create(command.PetSpecies.SpeciesId), command.PetSpecies.BreedId)
            .Value;
        
        var address = Address.Create(
            command.Address.City,
            command.Address.Street,
            command.Address.Building,
            command.Address.Description,
            command.Address.Country).Value;

        var requisite = Requisite.Create("name of Requisite", "description");
        
        var requisiteList = new PetRequisiteDetails([requisite.Value]);

        var pet = Pet.Create(PetId.NewPetId(),
            command.Name,
            petSpecies,
            description,
            command.Color,
            command.HealthInfo,
            address,
            command.Weight,
            command.Height,
            PhoneNumber.Create(command.OwnersPhoneNumber).Value,
            command.IsNeutered,
            command.DateOfBirth.ToUniversalTime(),
            command.IsVaccinated,
            command.HelpStatus,
            requisiteList,
            null);

        return pet.Value;
    }
}