using PetFriend.Core.Abstractions;

namespace PetFriend.Accounts.Application.RegisterVolunteer;

public record RegisterVolunteerCommand(
    string? Name,
    string? Surname,
    string? Patronymic,
    string UserName,
    string Email,
    string Password,
    int Experience) : ICommand;