namespace PetFriend.Accounts.Domain.TypeAccounts;

public class AdminAccount
{
    private AdminAccount() { }
    
    public AdminAccount(User user)
    {
        Id = Guid.NewGuid();
        User = user;
        UserId = user.Id;
    }
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public User User { get; init; }
}