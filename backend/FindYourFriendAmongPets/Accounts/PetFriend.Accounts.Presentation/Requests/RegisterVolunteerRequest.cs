using PetFriend.Accounts.Application.RegisterVolunteer;

namespace PetFriend.Accounts.Presentation.Requests;

public record RegisterVolunteerRequest(
    string? Name,
    string? Surname,
    string? Patronymic,
    string UserName,
    string Email,
    string Password,
    int Experience)
{
    public RegisterVolunteerCommand ToCommand() =>
        new(Name, Surname, Patronymic, UserName, Email, Password, Experience);
}