using Appointments.Application.Features.Chat.Commands.CreateChatRoom;
using Appointments.Application.Features.Chat.Queries.GetChatMessages;
using Appointments.Application.Features.Chat.Queries.GetUserChatRooms;
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
        var chatRoomId = await _mediator.Send(command);
        return Ok(chatRoomId);
    }
[AllowAnonymous]
    [HttpGet("messages/{chatRoomId}")]
    public async Task<IActionResult> GetChatMessages(Guid chatRoomId)
    {
        var messages = await _mediator.Send(new GetChatMessagesQuery { ChatRoomId = chatRoomId });
        return Ok(messages);
    }

    [HttpGet("my-rooms")]
    public async Task<IActionResult> GetMyChatRooms()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        var chatRooms = await _mediator.Send(new GetUserChatRoomsQuery { UserId = userId });
        return Ok(chatRooms);
    }
}
