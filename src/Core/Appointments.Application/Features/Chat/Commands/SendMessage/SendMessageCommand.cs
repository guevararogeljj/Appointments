using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Chat.Commands.SendMessage;

public class SendMessageCommand : IRequest<Response<Guid>>
{
    public Guid ChatRoomId { get; set; }
    public string? SenderId { get; set; }
    public string? ReceiverId { get; set; }
    public string? Message { get; set; }
}
