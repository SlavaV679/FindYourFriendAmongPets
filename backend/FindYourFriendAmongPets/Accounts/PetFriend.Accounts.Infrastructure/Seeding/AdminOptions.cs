namespace PetFriend.Accounts.Infrastructure.Seeding;

public class AdminOptions
{
    public const string ADMIN = nameof(ADMIN);
    
    public string UserName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string Patronymic { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}