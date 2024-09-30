using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.Application.Extensions;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace FindYourFriendAmongPets.Application.Volunteers.Commands.Delete;

public class DeleteVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(
        IVolunteerRepository volunteerRepository,
        IValidator<DeleteVolunteerCommand> validator,
        IUnitOfWork unitOfWork,
        ILogger<DeleteVolunteerHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteVolunteerCommand command,
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

        volunteerResult.Value.Delete();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Updated deleted with id {volunteerId}", id);

        return volunteerResult.Value.Id.Value;
    }
}