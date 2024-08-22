namespace FindYourFriendAmongPets.Application.Volunteers.Create;

public record VolunteerDto(
    string FirstName, 
    string LastName, 
    string? Patronymic,
    string Description,
    int ExperienceInYears,
    int CountPetsRealized,
    int CountPetsLookingForHome,
    int CountPetsHealing,
    string PhoneNumber);