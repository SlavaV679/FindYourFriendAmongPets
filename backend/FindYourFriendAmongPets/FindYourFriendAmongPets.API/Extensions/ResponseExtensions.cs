using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.API.Response;
using FindYourFriendAmongPets.Core.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FindYourFriendAmongPets.API.Extensions;

public static class ResponseExtensions
{
    public static ActionResult<T> ToResponse<T>(this Result<T, Error> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(Envelope.Ok(result.Value));

        var statusCode = result.Error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        var responseError = new ResponseError(result.Error.Code, result.Error.Message, null);
        
        var envelope = Envelope.Error([responseError]);

        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }
}