using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using Microsoft.Extensions.Logging;

namespace FindYourFriendAmongPets.Application.Volunteers.Create;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(IVolunteerRepository repository, ILogger<CreateVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(CreateVolunteerRequest request, CancellationToken token)
    {
        var fullName = FullName.Create(
            request.FullName.FirstName,
            request.FullName.LastName,
            request.FullName.Patronymic).Value;

        var description = Description.Create(request.Description).Value;

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;

        var requisitesForHelp = request.RequisitesForHelpDto?
            .Select(r => new RequisiteForHelp(Guid.NewGuid(), r.Name, r.Description)) ?? [];

        var socialNetworks = request.SocialNetworksDto?
            .Select(s => new SocialNetwork(Guid.NewGuid(), s.Title, s.Link)) ?? [];

        var volunteer = Volunteer.Create(VolunteerId.NewVolunteerId(),
            fullName,
            description,
            phoneNumber,
            requisitesForHelp,
            socialNetworks,
            request.ExperienceInYears
        );

        var result = await _repository.Add(volunteer.Value, token);

        var fullNameString = $"{volunteer.Value.FullName.FirstName} {volunteer.Value.FullName.LastName}";
        var id = volunteer.Value.Id.Value;
        
        _logger.LogInformation("Created volunteer '{fullNameString}' with id '{id}'", fullNameString, id);

        return (Guid)result.Value;
    }
}