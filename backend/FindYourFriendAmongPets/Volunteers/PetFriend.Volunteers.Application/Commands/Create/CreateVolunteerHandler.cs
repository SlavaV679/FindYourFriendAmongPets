using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFriend.Core.Abstractions;
using PetFriend.Core.Extensions;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects;
using PetFriend.SharedKernel.ValueObjects.Ids;
using PetFriend.Volunteers.Application.Database;
using PetFriend.Volunteers.Domain;
using PetFriend.Volunteers.Domain.Entities;
using PetFriend.Volunteers.Domain.ValueObject;

namespace PetFriend.Volunteers.Application.Commands.Create;

public class CreateVolunteerHandler: ICommandHandler<Guid, CreateVolunteerCommand>
{
    private readonly IVolunteerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteerRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<CreateVolunteerCommand> validator,
        ILogger<CreateVolunteerHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }
        
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        
        var volunteerResult = await _repository.GetByPhoneNumber(phoneNumber, cancellationToken);

        if (volunteerResult.IsSuccess)
        {
            return Errors.Model.AlreadyExist("Volunteer").ToErrorList();
        }

        var fullName = FullName.Create(
            command.FullName.FirstName,
            command.FullName.LastName,
            command.FullName.Patronymic).Value;

        var description = Description.Create(command.Description).Value;

        var requisitesForHelp = command.RequisitesForHelpDto?
            .Select(r => new RequisiteForHelp(Guid.NewGuid(), r.Name, r.Description)) ?? [];

        var socialNetworks = command.SocialNetworksDto?
            .Select(s => new SocialNetwork(Guid.NewGuid(), s.Title, s.Link)) ?? [];

        var volunteer = Volunteer.Create(VolunteerId.NewVolunteerId(),
            fullName,
            description,
            phoneNumber,
            requisitesForHelp,
            socialNetworks,
            command.ExperienceInYears
        );

        var result = await _repository.Add(volunteer.Value, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        var fullNameString = $"{volunteer.Value.FullName.FirstName} {volunteer.Value.FullName.LastName}";
        var id = volunteer.Value.Id.Value;

        _logger.LogInformation("Created volunteer '{fullNameString}' with id '{id}'", fullNameString, id);

        return (Guid)result.Value;
    }
}