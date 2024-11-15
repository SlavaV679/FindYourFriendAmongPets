using Microsoft.AspNetCore.Mvc;
using PetFriend.Accounts.Application.Register;
using PetFriend.Accounts.Presentation.Requests;
using PetFriend.Framework;

namespace PetFriend.Accounts.Presentation;

public class AccountsController : ApplicationController
{
    // //test
    // [Permission("species.create")]
    // [HttpPost("admin")]
    // public IActionResult CreatePet()
    // {
    //     return Ok();
    // }
    //
    // //test
    // [Permission("volunteer.create")]
    // [HttpPost("user")]
    // public IActionResult DeletePet()
    // {
    //     return Ok();
    // }
    //
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
    
        return Ok();
    }
    //
    // [HttpPost("login")]
    // public async Task<IActionResult> Login(
    //     [FromBody] LoginUserRequest request,
    //     [FromServices] LoginHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var result = await handler.Execute(request.ToCommand(), cancellationToken);
    //     if (result.IsFailure)
    //         return result.Error.ToResponse();
    //
    //     return Ok(result.Value);
    // }
    //
    // [HttpPost("refresh-token")]
    // public async Task<IActionResult> RefreshToken(
    //     [FromBody] RefreshTokenRequest request,
    //     [FromServices] RefreshTokenHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var result = await handler.Execute(new RefreshTokenCommand(request.AccessToken, request.RefreshToken),
    //         cancellationToken);
    //     
    //     if (result.IsFailure)
    //         return result.Error.ToResponse();
    //
    //     return Ok(result.Value);
    // }
    //
    // [Permission(Permissions.User.UpdateSocialLinks)]
    // [HttpPatch("{userId:guid}/social-links")]
    // public async Task<IActionResult> UpdateAccountSocialLinks(
    //     [FromBody] IEnumerable<SocialLinkDto> request,
    //     [FromRoute] Guid userId,
    //     [FromServices] UpdateAccountSocialLinksHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var result = await handler.Execute(new UpdateAccountSocialLinksCommand(userId, request), cancellationToken);
    //     if (result.IsFailure)
    //         return result.Error.ToResponse();
    //
    //     return Ok();
    // }
    //
    //
    // [Permission(Permissions.Volunteer.UpdateRequisites)]
    // [HttpPatch("{userId:guid}/requisites")]
    // public async Task<IActionResult> UpdateVolunteerRequisites(
    //     [FromBody] IEnumerable<RequisiteDto> request,
    //     [FromRoute] Guid userId,
    //     [FromServices] UpdateAccountRequisitesHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var result = await handler.Execute(new UpdateAccountRequisitesCommand(userId, request), cancellationToken);
    //     if (result.IsFailure)
    //         return result.Error.ToResponse();
    //
    //     return Ok();
    // }
    //
    // [Permission(Permissions.User.UpdateFullName)]
    // [HttpPatch("{userId:guid}/full-name")]
    // public async Task<IActionResult> UpdateFullName(
    //     [FromBody] FullNameDto request,
    //     [FromRoute] Guid userId,
    //     [FromServices] UpdateFullNameHandler handler,
    //     CancellationToken cancellationToken
    // )
    // {
    //     var result = await handler.Execute(new UpdateFullNameCommand(userId, request), cancellationToken);
    //     if (result.IsFailure)
    //         return result.Error.ToResponse();
    //
    //     return Ok();
    // }
    //
    // [HttpGet("{userId:guid}")]
    // public async Task<IActionResult> GetUserWithRoles(
    //     [FromRoute] Guid userId,
    //     [FromServices] GetUserWithRolesHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var result = await handler.Execute(new GetUserWithRolesQuery(userId), cancellationToken);
    //     if (result.IsFailure)
    //         return result.Error.ToResponse();
    //
    //     return Ok(result.Value);
    // }
    //
    // //test volunteer methods
    // [HttpPost("registration-volunteer")]
    // public async Task<IActionResult> CreateVolunteer(
    //     IAccountsUnitOfWork unitOfWork,
    //     IVolunteerAccountManager accountManager,
    //     UserManager<User> userManager,
    //     RoleManager<Role> roleManager,
    //     CancellationToken cancellationToken)
    // {
    //     var command = new RegisterUserCommand("test",
    //         "test2",
    //         "test3",
    //         "volunteerTest",
    //         "volunteerTest@gmail.com",
    //         "zxcVolunteer001");
    //
    //     var existsUserWithUserName = await userManager.FindByNameAsync(command.UserName);
    //     if (existsUserWithUserName != null)
    //         return BadRequest("уже есть але");
    //
    //     var role = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name == Roles.Volunteer, cancellationToken);
    //     if (role is null)
    //         return BadRequest("уже есть але");
    //
    //     var fullname = FullName.Create(command.Name, command.Surname, command.Patronymic).Value;
    //     var user = Domain.User.CreateParticipant(fullname, command.UserName, command.Email, role);
    //
    //     var result = await userManager.CreateAsync(user, command.Password);
    //     if (result.Succeeded == false)
    //     {
    //         return BadRequest("уже есть але");
    //     }
    //
    //     var participantAccount = new VolunteerAccount(5, [], user);
    //     await accountManager.CreateVolunteerAccountAsync(participantAccount, cancellationToken);
    //     await unitOfWork.SaveChanges(cancellationToken);
    //
    //     return Ok();
    // }
    //
    // //test
    // [HttpGet("volunteer-jwt")]
    // public async Task<IActionResult> GetVolunteerJwt(
    //     [FromServices] LoginHandler handler)
    // {
    //     var command = new LoginUserCommand("volunteerTest@gmail.com", "zxcVolunteer001");
    //
    //     var result = await handler.Execute(command);
    //     if (result.IsFailure)
    //         return result.Error.ToResponse();
    //     
    //     return Ok(result.Value);
    // }
}