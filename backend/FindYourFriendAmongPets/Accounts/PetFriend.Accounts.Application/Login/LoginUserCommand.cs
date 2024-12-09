using PetFriend.Core.Abstractions;

namespace PetFriend.Accounts.Application.Login;

public record LoginUserCommand(string Email, string Password) : ICommand;