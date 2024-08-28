namespace FindYourFriendAmongPets.Application.Volunteers.Create;

public record FullNameDto(
    string FirstName,
    string LastName,
    string? Patronymic);