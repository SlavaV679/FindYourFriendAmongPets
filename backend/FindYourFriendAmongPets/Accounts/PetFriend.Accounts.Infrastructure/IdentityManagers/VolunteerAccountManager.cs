using Microsoft.EntityFrameworkCore;
using PetFriend.Accounts.Application;
using PetFriend.Accounts.Domain.TypeAccounts;
using PetFriend.Accounts.Infrastructure.DbContexts;

namespace PetFriend.Accounts.Infrastructure.IdentityManagers;

public class VolunteerAccountManager(AccountsWriteDbContext context) : IVolunteerAccountManager
{
    public async Task CreateVolunteerAccountAsync(VolunteerAccount volunteerAccount, CancellationToken cancellationToken = default)
    {
        await context.Volunteers.AddAsync(volunteerAccount, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<VolunteerAccount?> GetVolunteerAccountByIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await context.Volunteers.FirstOrDefaultAsync(v => v.UserId == userId, cancellationToken);
    
    public async Task UpdateAsync(VolunteerAccount volunteerAccount, CancellationToken cancellationToken = default)
    {
        context.Volunteers.Attach(volunteerAccount);
        await context.SaveChangesAsync(cancellationToken);
    }
}