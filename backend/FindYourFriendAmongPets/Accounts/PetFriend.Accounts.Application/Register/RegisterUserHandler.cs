using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFriend.Accounts.Domain;
using PetFriend.Accounts.Domain.TypeAccounts;
using PetFriend.Core.Abstractions;
using PetFriend.Core.Extensions;
using PetFriend.Framework;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects;

namespace PetFriend.Accounts.Application.Register;

public class RegisterUserHandler(
    IAccountsUnitOfWork unitOfWork,
    IParticipantAccountManager accountManager,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IValidator<RegisterUserCommand> validator,
    ILogger<RegisterUserHandler> logger) : ICommandHandler<RegisterUserCommand>
{
    public async Task<UnitResult<ErrorList>> Handle(
        RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var existsUserWithUserName = await userManager.FindByNameAsync(command.UserName);
        if (existsUserWithUserName != null)
            return Errors.General.AlreadyExist("username").ToErrorList();

        var role = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name == Roles.Participant, cancellationToken);
        if (role is null)
            return Errors.General.NotFound().ToErrorList();

        var transaction = await unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var user = await CreateUser(command, role);
            if (user.IsFailure)
                return user.Error;

            var participantAccount = new ParticipantAccount(user.Value);
            await accountManager.CreateParticipantAccountAsync(participantAccount, cancellationToken);

            transaction.Commit();
            await unitOfWork.SaveChanges(cancellationToken);
            logger.LogInformation("User {UserName} has created a new account", command.UserName);
            
            //TODO обернуть ответ в Envelope
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            logger.Log(LogLevel.Critical, "Error during user registration {ex}", ex);
            return Error.Failure("Error.during.user.registration", "Error during user registration").ToErrorList();
        }
    }

    private async Task<Result<User, ErrorList>> CreateUser(RegisterUserCommand command, Role role)
    {
        FullName fullName;
        if (!string.IsNullOrWhiteSpace(command.Name) && !string.IsNullOrWhiteSpace(command.Surname))
            fullName = FullName.Create(command.Name, command.Surname, command.Patronymic).Value;
        else
            fullName = FullName.Create("name", "surname", null).Value;
            
        var user = User.CreateParticipant(fullName, command.UserName, command.Email, role);
        var result = await userManager.CreateAsync(user, command.Password);
        
        if (result.Succeeded != false) 
            return user;
        
        var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();
        return new ErrorList(errors);
    }
}