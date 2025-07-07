using MediatR;

namespace Appointments.Application.Features.Chat.Queries.GetUserChatRooms;

public class GetUserChatRoomsQuery : IRequest<IReadOnlyList<ChatRoomDto>>
{
    public string UserId { get; set; }
}
