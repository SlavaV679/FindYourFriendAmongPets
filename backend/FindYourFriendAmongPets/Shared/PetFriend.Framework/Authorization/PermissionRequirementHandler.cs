using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetFriend.Accounts.Contracts;

namespace PetFriend.Framework.Authorization;

public class PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<PermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionAttribute permission)
    {
        var userIdFromClaims = context.User.Claims.FirstOrDefault(); //c => c.Type == CustomClaims.Id);

        if (userIdFromClaims is null)
            return;

        if (!Guid.TryParse(userIdFromClaims.Value, out var userId))
        {
            context.Fail();
            return;
        }

        using var scope = serviceScopeFactory.CreateScope();
        var contract = scope.ServiceProvider.GetRequiredService<IAccountContract>();

        var permissions = await contract.GetPermissionsUserById(userId);
        if (permissions is null)
        {
            context.Fail();
            return;
        }

        if (permissions.Contains(permission.Code))
            context.Succeed(permission);
    }
}