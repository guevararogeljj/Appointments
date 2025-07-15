using Microsoft.AspNetCore.Authorization;
using Appointments.Application.Features.Appointments.Commands.CreateAppointment;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Application.Features.Appointments.Queries.GetAppointmentById;
using Appointments.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.API.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllAppointmentsQuery());
        if (result.Error != null)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetAppointmentByIdQuery { Id = id });
        if (result.Error != null)
        {
            return NotFound(result.Error);
        }
        return Ok(result.Result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Error != null)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Result);
    }
}
