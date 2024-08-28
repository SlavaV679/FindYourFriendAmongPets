using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.API.Extensions;
using FindYourFriendAmongPets.API.Response;
using FindYourFriendAmongPets.Application.Volunteers.Create;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FindYourFriendAmongPets.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler, [FromServices] IValidator<CreateVolunteerRequest> validator,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ValidateBadRequest();

        var response = await handler.Handle(request, cancellationToken);

        return response.ToResponse();
    }
}