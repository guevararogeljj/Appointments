using Appointments.Application.Features.Mailing.Commands;
using Appointments.Application.Features.Mailing.Commands.SenderMailingsCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.API.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class SenderMailingController : ControllerBase
{
    private readonly IMediator _mediator;
    public SenderMailingController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Retrieves the sender mailing information based on the provided email.
    /// </summary>
    /// <param name="email">The email address of the sender.</param>
    /// <returns>A string containing the sender mailing information.</returns>
    /// <response code="200">Returns the sender mailing information.</response>
    /// <response code="404">If the sender mailing information is not found.</response>
    [HttpPost]
    [Route("SenderMailing")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    
    public async Task<IActionResult> SenderMailing(SenderMailingCommand request)
    {
        var result = await _mediator.Send(request);
        if (result.Result)
        {
            return Ok(result);
        }
        ///problem details
        
        return Problem(
            statusCode: 404,
            title: "Not Found",
            detail: result.Error?.Message ?? "The requested resource was not found."
        );
    }

    [HttpPost]
    [Route("SenderMailings")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    
    public async Task<IActionResult> SenderMailings(SenderMailingsCommand request)
    {
        var result = await _mediator.Send(request);
        if (result.Result)
        {
            return Ok(result);
        }
        ///problem details
        return Problem(
            statusCode: 404,
            title: "Not Found",
            detail: result.Error?.Message ?? "The requested resource was not found."
        );
    }
}