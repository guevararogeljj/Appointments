using Appointments.Application.Features.Roles.Commands.CreateRole;
using Appointments.Application.Features.Roles.Commands.DeleteRole;
using Appointments.Application.Features.Roles.Commands.UpdateRole;
using Appointments.Application.Features.Roles.Queries.GetAllRoles;
using Appointments.Application.Features.Roles.Queries.GetRoleById;
using Appointments.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.API.Controllers.V1;

[Authorize(Roles = "Administrator")]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllRolesQuery());
        if (result.Error != null)
        {
            return Problem(statusCode: int.Parse(result.Error.Code), title: result.Error.Code, detail: result.Error.Message);
        }
        return Ok(result.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetRoleByIdQuery { Id = id });
        if (result.Error != null)
        {
            return Problem(statusCode: int.Parse(result.Error.Code), title: result.Error.Code, detail: result.Error.Message);
        }
        return Ok(result.Result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Error != null)
        {
            return Problem(statusCode: int.Parse(result.Error.Code), title: result.Error.Code, detail: result.Error.Message);
        }
        return Ok(result.Result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateRoleCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Error != null)
        {
            return Problem(statusCode: int.Parse(result.Error.Code), title: result.Error.Code, detail: result.Error.Message);
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteRoleCommand { Id = id });
        if (result.Error != null)
        {
            return Problem(statusCode: int.Parse(result.Error.Code), title: result.Error.Code, detail: result.Error.Message);
        }
        return NoContent();
    }
}
