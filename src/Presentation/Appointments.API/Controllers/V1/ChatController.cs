using Appointments.Application.Features.Chat.Commands.CreateChatRoom;
using Appointments.Application.Features.Chat.Queries.GetChatMessages;
using Appointments.Application.Features.Chat.Queries.GetUserChatRooms;
using Appointments.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Appointments.API.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-room")]
    public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Error != null)
        {
            return Problem(statusCode: 400, title: result.Error.Code, detail: result.Error.Message);
        }
        return Ok(result.Result);
    }
    [AllowAnonymous]
    [HttpGet("messages/{chatRoomId}")]
    public async Task<IActionResult> GetChatMessages(Guid chatRoomId)
    {
        var result = await _mediator.Send(new GetChatMessagesQuery { ChatRoomId = chatRoomId });
        if (result.Error != null)
        {
            return Problem(statusCode: 404, title: result.Error.Code, detail: result.Error.Message);
        }
        return Ok(result.Result);
    }

    [HttpGet("my-rooms")]
    public async Task<IActionResult> GetMyChatRooms()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Problem(statusCode: 401, title: "Unauthorized", detail: "User not found");
        }
        var result = await _mediator.Send(new GetUserChatRoomsQuery { UserId = userId });
        if (result.Error != null)
        {
            return Problem(statusCode: 404, title: result.Error.Code, detail: result.Error.Message);
        }
        return Ok(result.Result);
    }
}
