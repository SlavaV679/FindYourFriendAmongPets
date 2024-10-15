using FindYourFriendAmongPets.API.Controllers.Queries.Requests;
using FindYourFriendAmongPets.Application.Volunteers.Queries.GetVolunteerById;
using FindYourFriendAmongPets.Application.Volunteers.Queries.GetVolunteersWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace FindYourFriendAmongPets.API.Controllers.Queries;

public class VolunteerController: ApplicationController
{
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