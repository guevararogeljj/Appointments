using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Entities.Chat;
using MediatR;

namespace Appointments.Application.Features.Chat.Commands.CreateChatRoom;

public class CreateChatRoomCommandHandler : IRequestHandler<CreateChatRoomCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateChatRoomCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateChatRoomCommand request, CancellationToken cancellationToken)
    {
        // Check if a chat room already exists between these two users
        var existingChatRoom = await _unitOfWork.ChatRooms.GetChatRoomByUsersAsync(request.User1Id, request.User2Id);
        if (existingChatRoom != null)
        {
            return existingChatRoom.Id;
        }

        var chatRoom = new ChatRoom
        {
            Id = Guid.NewGuid(),
            User1Id = request.User1Id,
            User2Id = request.User2Id
        };

        await _unitOfWork.ChatRooms.AddAsync(chatRoom);
        await _unitOfWork.CompleteAsync();

        return chatRoom.Id;
    }
}
