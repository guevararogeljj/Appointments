using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Chat.Commands.CreateChatRoom;

public class CreateChatRoomCommand : IRequest<Response<Guid>>
{
    public string? User1Id { get; set; }
    public string? User2Id { get; set; }
}
