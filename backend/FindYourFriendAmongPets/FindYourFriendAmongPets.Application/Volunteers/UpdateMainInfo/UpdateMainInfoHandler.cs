using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using Microsoft.Extensions.Logging;

namespace FindYourFriendAmongPets.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteerRepository volunteerRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
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

        //var result = await _volunteerRepository.Save(volunteerResult.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        var fullNameString = $"{volunteerResult.Value.FullName.FirstName} {volunteerResult.Value.FullName.LastName}";
        
        _logger.LogInformation(
            "Updated volunteer {fullNameString}, {description}, {phoneNumber}, {request.Dto.ExperienceInYears} with id {volunteerId}",
            fullNameString,
            description,
            phoneNumber,
            request.Dto.ExperienceInYears,
            request.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}