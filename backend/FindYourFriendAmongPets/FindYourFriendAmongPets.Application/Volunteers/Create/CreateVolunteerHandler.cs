using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Application.Volunteers.Create;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _repository;

    public CreateVolunteerHandler(IVolunteerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid, Error>> Handle(CreateVolunteerRequest request, CancellationToken token)
    {
        var fullName = FullName.Create(
            request.FirstName,
            request.LastName,
            request.Patronymic).Value;

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

        return (Guid)result.Value;
    }
}