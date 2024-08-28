using FindYourFriendAmongPets.API.Extensions;
using FindYourFriendAmongPets.API.Response;
using FindYourFriendAmongPets.Application.Volunteers.Create;
using FindYourFriendAmongPets.Core.Shared;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace FindYourFriendAmongPets.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request, cancellationToken);

        return response.ToResponse();
    }
}