using MediatR;

namespace Appointments.Application.Features.Chat.Commands.SendMessage;

public class SendMessageCommand : IRequest<Guid>
{
    public Guid ChatRoomId { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public string Message { get; set; }
}
