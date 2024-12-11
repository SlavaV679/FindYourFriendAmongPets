using PetFriend.Core.Abstractions;

namespace PetFriend.Accounts.Application.Register;

public record RegisterUserCommand(
    string? Name,
    string? Surname,
    string? Patronymic,
    string UserName,
    string Email,
    string Password) : ICommand;