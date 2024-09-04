namespace FindYourFriendAmongPets.Application.Volunteers.Shared;

public record FullNameDto(
    string FirstName,
    string LastName,
    string? Patronymic);