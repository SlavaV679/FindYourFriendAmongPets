﻿using CSharpFunctionalExtensions;
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

namespace PetFriend.Accounts.Application.RegisterVolunteer;

public class RegisterVolunteerHandler(
    IAccountsUnitOfWork unitOfWork,
    IVolunteerAccountManager accountManager,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IValidator<RegisterVolunteerCommand> validator,
    ILogger<RegisterVolunteerHandler> logger) : ICommandHandler<Guid, RegisterVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(RegisterVolunteerCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var existsUserWithUserName = await userManager.FindByNameAsync(command.UserName);
        if (existsUserWithUserName != null)
            return Errors.General.AlreadyExist("username").ToErrorList();

        var role = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name == Roles.Volunteer, cancellationToken);
        if (role is null)
            return Errors.General.NotFound().ToErrorList();

        var transaction = await unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var user = await CreateUser(command, role);
            if (user.IsFailure)
                return user.Error;

            var volunteerAccount = new VolunteerAccount(command.Experience, user.Value);
            await accountManager.CreateVolunteerAccountAsync(volunteerAccount, cancellationToken);

            transaction.Commit();
            await unitOfWork.SaveChanges(cancellationToken);
            logger.LogInformation("User {UserName} has created as a new volunteer account", command.UserName);

            return volunteerAccount.Id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            logger.Log(LogLevel.Critical, "Error during user registration {ex}", ex);
            return Error.Failure("Error.during.user.registration", "Error during user registration").ToErrorList();
        }
    }

    private async Task<Result<User, ErrorList>> CreateUser(RegisterVolunteerCommand command, Role role)
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