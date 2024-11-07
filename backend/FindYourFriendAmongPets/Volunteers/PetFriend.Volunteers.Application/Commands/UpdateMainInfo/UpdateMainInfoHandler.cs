using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFriend.Core.Abstractions;
using PetFriend.Core.Extensions;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects;
using PetFriend.SharedKernel.ValueObjects.Ids;
using PetFriend.Volunteers.Application.Database;
using PetFriend.Volunteers.Domain.ValueObject;

namespace PetFriend.Volunteers.Application.Commands.UpdateMainInfo;

public class UpdateMainInfoHandler : ICommandHandler<Guid, UpdateMainInfoCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateMainInfoCommand> _validator;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteerRepository volunteerRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateMainInfoCommand> validator,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var id = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _volunteerRepository.GetById(id, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var fullName = FullName.Create(
            command.FullName.FirstName,
            command.FullName.LastName,
            command.FullName.Patronymic).Value;

        var description = Description.Create(command.Description).Value;

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        volunteerResult.Value.UpdateMainInfo(fullName, description, phoneNumber, command.ExperienceInYears);

        //var result = await _volunteerRepository.Save(volunteerResult.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        var fullNameString = $"{volunteerResult.Value.FullName.FirstName} {volunteerResult.Value.FullName.LastName}";

        _logger.LogInformation(
            "Updated volunteer {fullNameString}, {description}, {phoneNumber}, {command.ExperienceInYears} with id {volunteerId}",
            fullNameString,
            description,
            phoneNumber,
            command.ExperienceInYears,
            command.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}