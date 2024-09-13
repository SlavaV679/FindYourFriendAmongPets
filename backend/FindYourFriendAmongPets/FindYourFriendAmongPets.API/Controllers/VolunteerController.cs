using FindYourFriendAmongPets.API.Controllers.Requests;
using FindYourFriendAmongPets.API.Extensions;
using FindYourFriendAmongPets.API.Processors;
using FindYourFriendAmongPets.Application.Volunteers.AddPet;
using FindYourFriendAmongPets.Application.Volunteers.Create;
using FindYourFriendAmongPets.Application.Volunteers.Delete;
using FindYourFriendAmongPets.Application.Volunteers.UpdateMainInfo;
using FindYourFriendAmongPets.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FindYourFriendAmongPets.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromServices] IOptions<MinioOptions> options,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var v = options.Value.Endpoint;
        var response = await handler.Handle(request.ToCommand(), cancellationToken);

        return response.Error.ToResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteVolunteerCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoRequest request,
        [FromServices] UpdateMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid id,
        [FromForm] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        //нужен валидатор, который вручную проверит входные данные, так как автовалидатор не работает
        // с атрибутом [FromForm]
        // [FromServices] IValidator<AddPetRequest> validator,
        CancellationToken cancellationToken)
    {
        await using var fileProcessor = new FormFileProcessor();

        var fileCommands = fileProcessor.Process(request.Files);

        var command = request.ToCommand(id, fileCommands);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}