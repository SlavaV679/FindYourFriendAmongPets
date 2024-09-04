using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using Microsoft.Extensions.Logging;

namespace FindYourFriendAmongPets.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = VolunteerId.Create(request.VolunteerId);
        
        var volunteerResult = await _volunteerRepository.GetById(id, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var fullName = FullName.Create(
            request.Dto.FullName.FirstName,
            request.Dto.FullName.LastName,
            request.Dto.FullName.Patronymic).Value;

        var description = Description.Create(request.Dto.Description).Value;

        var phoneNumber = PhoneNumber.Create(request.Dto.PhoneNumber).Value;

        volunteerResult.Value.UpdateMainInfo(fullName, description, phoneNumber, request.Dto.ExperienceInYears);

        var result = await _volunteerRepository.Save(volunteerResult.Value, cancellationToken);

        var fullNameString = $"{volunteerResult.Value.FullName.FirstName} {volunteerResult.Value.FullName.LastName}";
        
        _logger.LogInformation(
            "Updated volunteer {fullNameString}, {description}, {phoneNumber}, {request.Dto.ExperienceInYears} with id {volunteerId}",
            fullNameString,
            description,
            phoneNumber,
            request.Dto.ExperienceInYears,
            request.VolunteerId);

        return result;
    }
}