namespace PetFriend.Core.Dtos;

public record FullNameDto(
    string FirstName,
    string LastName,
    string? Patronymic);