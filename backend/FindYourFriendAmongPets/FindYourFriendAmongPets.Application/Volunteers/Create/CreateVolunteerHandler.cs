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
    public async Task<Result<Guid, Error>> Handle(VolunteerDto volunteerDto, CancellationToken token)
    {
        var fullName = FullName.Create(
            volunteerDto.FirstName,
            volunteerDto.LastName,
            volunteerDto.Patronymic);

        if (fullName.IsFailure)
            return fullName.Error;

        var phoneNumber = PhoneNumber.Create(volunteerDto.PhoneNumber);
        
        if (phoneNumber.IsFailure)
            return phoneNumber.Error;

        var volunteer = new Volunteer(VolunteerId.NewVolunteerId(),
            fullName.Value,
            volunteerDto.Description,
            volunteerDto.ExperienceInYears,
            volunteerDto.CountPetsRealized,
            volunteerDto.CountPetsLookingForHome,
            volunteerDto.CountPetsHealing,
            phoneNumber.Value
        );
        
        var result = await _repository.Add(volunteer, token);
        
        return (Guid)result.Value;
    }
}