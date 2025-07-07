using MediatR;
using Microsoft.AspNetCore.SignalR;
using Appointments.Application.Features.Chat.Commands.SendMessage;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Appointments.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private static readonly Dictionary<string, string> UserConnections = new Dictionary<string, string>();
    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendMessage(Guid chatRoomId, string receiverId, string message)
    {
        var senderId =Context.User!.Claims.Where(x => x.Type == "idUser").FirstOrDefault()!.Value; 
            var command = new SendMessageCommand
        {
            ChatRoomId = chatRoomId,
            SenderId = senderId,
            ReceiverId = receiverId,
            Message = message
        };
        
        await _mediator.Send(command);

        // Send to sender and receiver
        await Clients.User(senderId).SendAsync("ReceiveMessage", chatRoomId, senderId, receiverId, message, DateTime.UtcNow);
        await Clients.User(receiverId).SendAsync("ReceiveMessage", chatRoomId, senderId, receiverId, message, DateTime.UtcNow);
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User!.Claims.Where(x => x.Type == "idUser").FirstOrDefault()!.Value;
        if (userId != null)
        {
            UserConnections[userId] = Context.ConnectionId;
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId =Context.User!.Claims.Where(x => x.Type == "idUser").FirstOrDefault()!.Value;
        if (userId != null)
        {
            UserConnections.Remove(userId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}
