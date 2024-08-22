using FindYourFriendAmongPets.Application.Volunteers.Create;
using FindYourFriendAmongPets.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FindYourFriendAmongPets.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] VolunteerDto request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request, cancellationToken);

        return Ok(response.Value);
    }
}