using Microsoft.AspNetCore.Authorization;
using Appointments.Application.Features.Patients.Commands.CreatePatient;
using Appointments.Application.Features.Patients.Queries.GetAllPatients;
using Appointments.Application.Features.Patients.Queries.GetPatientById;
using Appointments.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.API.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllPatientsQuery());
        if (result.Error != null)
        {
            return Problem(statusCode: int.Parse(result.Error.Code), title: result.Error.Code, detail: result.Error.Message);
        }
        return Ok(result.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetPatientByIdQuery { Id = id });
        if (result.Error != null)
        {
            return Problem(statusCode: int.Parse(result.Error.Code), title: result.Error.Code, detail: result.Error.Message);
        }
        return Ok(result.Result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePatientCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Error != null)
        {
            return Problem(statusCode: int.Parse(result.Error.Code), title: result.Error.Code, detail: result.Error.Message);
        }
        return Ok(result.Result);
    }
}
