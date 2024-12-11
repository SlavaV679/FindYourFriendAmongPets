namespace PetFriend.Accounts.Domain.TypeAccounts;

public class ParticipantAccount
{
    private ParticipantAccount() { }

    public ParticipantAccount(User user)
    {
        Id = Guid.NewGuid(); 
        User = user;
    }
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public User User { get; init; }
}