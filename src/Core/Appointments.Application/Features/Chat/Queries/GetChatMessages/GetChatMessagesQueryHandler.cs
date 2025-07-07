using Appointments.Application.Contracts.Persistence;
using Appointments.Application.Features.Users;
using MediatR;

namespace Appointments.Application.Features.Chat.Queries.GetChatMessages;

public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, IReadOnlyList<ChatMessageDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetChatMessagesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ChatMessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _unitOfWork.ChatMessages.GetMessagesByChatRoomIdAsync(request.ChatRoomId);

        return messages.Select(m => new ChatMessageDto
        {
            Id = m.Id,
            ChatRoomId = m.ChatRoomId,
            Sender = new UserDto { Id = m.Sender.Id, UserName = m.Sender.UserName, Email = m.Sender.Email, FirstName = m.Sender.FirstName, LastName = m.Sender.LastName },
            Receiver = new UserDto { Id = m.Receiver.Id, UserName = m.Receiver.UserName, Email = m.Receiver.Email, FirstName = m.Receiver.FirstName, LastName = m.Receiver.LastName },
            Message = m.Message,
            Timestamp = m.Timestamp
        }).OrderByDescending(x=>x.Timestamp).ToList();
    }
}
