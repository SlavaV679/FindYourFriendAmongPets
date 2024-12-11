using PetFriend.Accounts.Domain.TypeAccounts;
using PetFriend.Accounts.Infrastructure.DbContexts;

namespace PetFriend.Accounts.Infrastructure.IdentityManagers;

public class AdminAccountManager(AccountsWriteDbContext context) 
{
    public async Task CreateAdminAccountAsync(AdminAccount adminAccount, CancellationToken cancellationToken = default)
    {
        await context.Admins.AddAsync(adminAccount, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}