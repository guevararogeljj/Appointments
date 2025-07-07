using Appointments.Application.Features.Roles.Commands.CreateRole;
using Appointments.Application.Features.Roles.Commands.DeleteRole;
using Appointments.Application.Features.Roles.Commands.UpdateRole;
using Appointments.Application.Features.Roles.Queries.GetAllRoles;
using Appointments.Application.Features.Roles.Queries.GetRoleById;
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
        var dtos = await _mediator.Send(new GetAllRolesQuery());
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var dto = await _mediator.Send(new GetRoleByIdQuery { Id = id });
        if (dto == null)
        {
            return NotFound();
        }
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateRoleCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteRoleCommand { Id = id });
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }
}
