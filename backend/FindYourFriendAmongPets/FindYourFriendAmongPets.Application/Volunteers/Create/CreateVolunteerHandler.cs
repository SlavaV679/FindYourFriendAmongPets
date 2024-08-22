using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Models;

namespace FindYourFriendAmongPets.Application.Volunteers.Create;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _repository;

    public CreateVolunteerHandler(IVolunteerRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<VolunteerId>> Handle(VolunteerDto volunteerDto, CancellationToken token)
    {
        var fullName = FullName.Create(
            volunteerDto.FirstName,
            volunteerDto.LastName,
            volunteerDto.Patronymic);

        if (fullName.IsFailure)
            return Result.Failure<VolunteerId>(fullName.Error);

        var phoneNumber = PhoneNumber.Create(volunteerDto.PhoneNumber);
        
        if (phoneNumber.IsFailure)
            return Result.Failure<VolunteerId>(phoneNumber.Error);

        var volunteer = new Volunteer(VolunteerId.NewVolunteerId(),
            fullName.Value,
            volunteerDto.Description,
            volunteerDto.ExperienceInYears,
            volunteerDto.CountPetsRealized,
            volunteerDto.CountPetsLookingForHome,
            volunteerDto.CountPetsHealing,
            phoneNumber.Value
        );
        
        var volunteerId = await _repository.Add(volunteer, token);
        
        return Result.Success(volunteerId);
    }
}