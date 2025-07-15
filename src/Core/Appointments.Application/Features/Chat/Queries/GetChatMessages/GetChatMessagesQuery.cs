using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Chat.Queries.GetChatMessages;

public class GetChatMessagesQuery : IRequest<Response<IReadOnlyList<ChatMessageDto>>>
{
    public Guid ChatRoomId { get; set; }
}
