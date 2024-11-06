using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFriend.Framework;
using PetFriend.Volunteers.Application.Commands.AddPet;
using PetFriend.Volunteers.Application.Commands.Create;
using PetFriend.Volunteers.Application.Commands.Delete;
using PetFriend.Volunteers.Application.Commands.UpdateMainInfo;
using PetFriend.Volunteers.Application.Commands.UploadFilesToPet;
using PetFriend.Volunteers.Application.Queries.GetVolunteerById;
using PetFriend.Volunteers.Application.Queries.GetVolunteersWithPagination;
using PetFriend.Volunteers.Presentation.Processors;
using PetFriend.Volunteers.Presentation.Volunteers.Requests;

namespace PetFriend.Volunteers.Presentation.Volunteers;

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
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
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
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/pet/{petId:guid}/files")]
    public async Task<ActionResult> UploadFilesToPet(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadFilesToPetHandler handler,
        CancellationToken cancellationToken)
    {
        await using var fileProcessor = new FormFileProcessor();
        var fileDtos = fileProcessor.Process(files);

        var command = new UploadFilesToPetCommand(id, petId, fileDtos);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpGet("dapper")]
    public async Task<ActionResult> GetDapper(
        [FromQuery] GetFilteredVolunteersWithPaginationRequest request,
        [FromServices] GetVolunteersWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpGet("{id:guid}/dapper")]
    public async Task<ActionResult> GetByIdDapper(
        [FromRoute] Guid id,
        // [FromQuery] GetVolunteerByIdRequest request,
        [FromServices] GetVolunteerByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetVolunteerByIdQuery(id);
        
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }
}