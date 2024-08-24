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
            request.Patronymic);

        if (fullName.IsFailure)
            return fullName.Error;

        var description = Description.Create(request.Description);
        
        if (description.IsFailure)
            return description.Error;
        
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        
        if (phoneNumber.IsFailure)
            return phoneNumber.Error;

        var volunteer = new Volunteer(VolunteerId.NewVolunteerId(),
            fullName.Value,
            description.Value,
            request.ExperienceInYears,
            request.CountPetsRealized,
            request.CountPetsLookingForHome,
            request.CountPetsHealing,
            phoneNumber.Value
        );

        var requisitesForHelp = request.RequisitesForHelpDto?
            .Select(r => new RequisiteForHelp(Guid.NewGuid(), r.Name, r.Description));

        var socialNetworks = request.SocialNetworksDto?
            .Select(s => new SocialNetwork(Guid.NewGuid(), s.Title, s.Link));
        
        volunteer.AddRequisitesForHelp(requisitesForHelp);
        
        volunteer.AddSocialNetwork(socialNetworks);
        
        var result = await _repository.Add(volunteer, token);
        
        return (Guid)result.Value;
    }
}