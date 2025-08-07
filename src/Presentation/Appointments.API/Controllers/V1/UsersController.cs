using Appointments.Application.Features.Users.Commands.DeleteUser;
using Appointments.Application.Features.Users.Commands.UpdateUser;
using Appointments.Application.Features.Users.Queries.GetAllUsers;
using Appointments.Application.Features.Users.Queries.GetUserById;
using Appointments.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.API.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        if (result.Error != null)
        {
            return Problem(statusCode: int.Parse(result.Error.Code), title: result.Error.Code, detail: result.Error.Message);
        }
        return Ok(result.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
        if (result.Error != null)
        {
            return Problem(statusCode: int.Parse(result.Error.Code), title: result.Error.Code, detail: result.Error.Message);
        }
        return Ok(result.Result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
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
        var result = await _mediator.Send(new DeleteUserCommand { Id = id });
        if (result.Error != null)
        {
            return Problem(statusCode: int.Parse(result.Error.Code), title: result.Error.Code, detail: result.Error.Message);
        }
        return NoContent();
    }
}
