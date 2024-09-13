using FindYourFriendAmongPets.API.Contracts;
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
        var response = await handler.Handle(request, cancellationToken);

        return response.ToResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        [FromServices] IValidator<DeleteVolunteerRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new DeleteVolunteerRequest(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var response = await handler.Handle(request, cancellationToken);

        return response.ToResponse();
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoDto dto,
        [FromServices] UpdateMainInfoHandler handler,
        [FromServices] IValidator<UpdateMainInfoRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new UpdateMainInfoRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await handler.Handle(request, cancellationToken);

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
        var fileDtos = fileProcessor.Process(request.Files);

            var command = new AddPetCommand(
                id,
                request.Name,
                request.PetSpecies,
                request.Description,
                request.Color,
                request.HealthInfo,
                request.Address,
                request.Weight,
                request.Height,
                request.OwnersPhoneNumber,
                request.IsNeutered,
                request.DateOfBirth,
                request.IsVaccinated,
                request.HelpStatus,
                //request.RequisiteDetails,
                fileDtos);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
    }
}