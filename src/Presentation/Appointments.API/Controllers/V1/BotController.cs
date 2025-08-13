using Appointments.Application.Features.Bots.BotDefault.Queries;
using Appointments.Application.ML;
using Appointments.Application.ML.Commands;
using Appointments.Application.ML.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.API.Controllers.V1;

[AllowAnonymous]
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

    [HttpPost("train")]
    public async Task<IActionResult> TrainModel([FromBody] TrainModelCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("predict")]
    public async Task<IActionResult> Predict([FromBody] ModelInput input)
    {
        var query = new PredictQuery(input);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}