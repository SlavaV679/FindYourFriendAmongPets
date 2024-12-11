using PetFriend.Accounts.Application.Login;

namespace PetFriend.Accounts.Presentation.Requests;

public record LoginUserRequest(string Email, string Password)
{
    public LoginUserCommand ToCommand() => new(Email, Password);
}