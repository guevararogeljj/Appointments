using Appointments.Application.Contracts.Persistence;
using Appointments.Application.Features.Users;
using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Chat.Queries.GetUserChatRooms;

public class GetUserChatRoomsQueryHandler : IRequestHandler<GetUserChatRoomsQuery, Response<IReadOnlyList<ChatRoomDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserChatRoomsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<IReadOnlyList<ChatRoomDto>>> Handle(GetUserChatRoomsQuery request, CancellationToken cancellationToken)
    {
        var chatRooms = await _unitOfWork.ChatRooms.GetUserChatRoomsAsync(request.UserId);

        var dtos = chatRooms.Select(cr => new ChatRoomDto
        {
            Id = cr.Id,
            User1 = new UserDto { Id = cr.User1.Id, UserName = cr.User1.UserName, Email = cr.User1.Email, FirstName = cr.User1.FirstName, LastName = cr.User1.LastName },
            User2 = new UserDto { Id = cr.User2.Id, UserName = cr.User2.UserName, Email = cr.User2.Email, FirstName = cr.User2.FirstName, LastName = cr.User2.LastName }
        }).ToList();

        return new Response<IReadOnlyList<ChatRoomDto>> { Result = dtos };
    }
}
