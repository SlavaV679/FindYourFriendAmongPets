using FindYourFriendAmongPets.API.Controllers.Queries.Requests;
using FindYourFriendAmongPets.Application.Volunteers.Queries.GetPetsWithPaginationQuery;
using Microsoft.AspNetCore.Mvc;

namespace FindYourFriendAmongPets.API.Controllers.Queries;

public class PetsController: ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        [FromServices] GetPetsWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpGet("dapper")]
    public async Task<ActionResult> GetDapper(
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        [FromServices] GetPetsWithPaginationHandlerDapper handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }
}