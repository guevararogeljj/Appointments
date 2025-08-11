using Appointments.Application.Features.Bots.BotDefault.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.API.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BotController : ControllerBase
{
    private readonly IMediator _mediator;
    public BotController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("BotDefault")]
    public async Task<IActionResult> GetBotDefaultResponse([FromQuery] BotDefaultQuery request)
    {
        var response = await _mediator.Send(request);
        
        if (response.Error != null)
        {
            return Problem(statusCode: int.Parse(response.Error.Code), title: response.Error.Code, detail: response.Error.Message);
        }
        
        return Ok(response.Result);
    }
}