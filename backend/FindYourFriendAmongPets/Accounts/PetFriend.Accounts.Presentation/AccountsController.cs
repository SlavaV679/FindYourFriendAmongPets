﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PetFriend.Accounts.Application.Login;
using PetFriend.Accounts.Application.RefreshTokens;
using PetFriend.Accounts.Application.Register;
using PetFriend.Accounts.Application.RegisterVolunteer;
using PetFriend.Accounts.Presentation.Requests;
using PetFriend.Framework;
using PetFriend.Framework.Authorization;

namespace PetFriend.Accounts.Presentation;

public class AccountsController : ApplicationController
{
    //permissions test
    [Permission("volunteer.create")]
    [HttpPost("permissionstest")]
    public IActionResult TestPermissions()
    {
        return Ok("Permissions test completed successfully");
    }
    
    [HttpPost("registration")]
    public async Task<ActionResult<Guid>> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
    
        return Ok(result.Value);
    }
    
    [HttpPost("registration-volunteer")]
    public async Task<ActionResult<Guid>> RegisterVolunteer(
        [FromBody] RegisterVolunteerRequest request,
        [FromServices] RegisterVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
    
        return Ok(result.Value);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
    
        return Ok(result.Value);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }
        
        var result = await handler.Handle(new RefreshTokensCommand(Guid.Parse(refreshToken)),
            cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
    
        HttpContext.Response.Cookies.Append("refreshToken", result.Value.RefreshToken.ToString());
        
        return Ok(result.Value);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        HttpContext.Response.Cookies.Delete("refreshToken");
        //TODO почистить рефреш токен из базы данных
        return Ok("");
    }
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