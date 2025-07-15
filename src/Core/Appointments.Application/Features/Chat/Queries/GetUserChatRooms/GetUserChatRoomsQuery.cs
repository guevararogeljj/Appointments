using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Chat.Queries.GetUserChatRooms;

public class GetUserChatRoomsQuery : IRequest<Response<IReadOnlyList<ChatRoomDto>>>
{
    public string UserId { get; set; }
}
