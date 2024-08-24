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
    public async Task<Result<Guid, Error>> Handle(CreateVolunteerRequest createVolunteerRequest, CancellationToken token)
    {
        var fullName = FullName.Create(
            createVolunteerRequest.FirstName,
            createVolunteerRequest.LastName,
            createVolunteerRequest.Patronymic);

        if (fullName.IsFailure)
            return fullName.Error;

        var phoneNumber = PhoneNumber.Create(createVolunteerRequest.PhoneNumber);
        
        if (phoneNumber.IsFailure)
            return phoneNumber.Error;

        var volunteer = new Volunteer(VolunteerId.NewVolunteerId(),
            fullName.Value,
            createVolunteerRequest.Description,
            createVolunteerRequest.ExperienceInYears,
            createVolunteerRequest.CountPetsRealized,
            createVolunteerRequest.CountPetsLookingForHome,
            createVolunteerRequest.CountPetsHealing,
            phoneNumber.Value
        );

        var requisitesForHelp = createVolunteerRequest.RequisitesForHelpDto?
            .Select(r => new RequisiteForHelp(Guid.NewGuid(), r.Name, r.Description));

        var socialNetworks = createVolunteerRequest.SocialNetworksDto?
            .Select(s => new SocialNetwork(Guid.NewGuid(), s.Title, s.Link));
        
        volunteer.AddRequisitesForHelp(requisitesForHelp);
        
        volunteer.AddSocialNetwork(socialNetworks);
        
        var result = await _repository.Add(volunteer, token);
        
        return (Guid)result.Value;
    }
}