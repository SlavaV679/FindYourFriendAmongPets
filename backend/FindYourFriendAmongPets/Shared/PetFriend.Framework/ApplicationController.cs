using Microsoft.AspNetCore.Mvc;
using PetFriend.Core.Models;

namespace PetFriend.Framework;

[ApiController]
[Route("[controller]")]
public abstract class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);
        
        return base.Ok(envelope);
    }
}