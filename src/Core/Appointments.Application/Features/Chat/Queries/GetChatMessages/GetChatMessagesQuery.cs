using MediatR;

namespace Appointments.Application.Features.Chat.Queries.GetChatMessages;

public class GetChatMessagesQuery : IRequest<IReadOnlyList<ChatMessageDto>>
{
    public Guid ChatRoomId { get; set; }
}
