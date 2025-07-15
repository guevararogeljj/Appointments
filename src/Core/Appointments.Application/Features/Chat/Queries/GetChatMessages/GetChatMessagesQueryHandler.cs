using Appointments.Application.Contracts.Persistence;
using Appointments.Application.Features.Users;
using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Chat.Queries.GetChatMessages;

public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, Response<IReadOnlyList<ChatMessageDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetChatMessagesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<IReadOnlyList<ChatMessageDto>>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _unitOfWork.ChatMessages.GetMessagesByChatRoomIdAsync(request.ChatRoomId);

        var dtos = messages.Select(m => new ChatMessageDto
        {
            Id = m.Id,
            ChatRoomId = m.ChatRoomId,
            Sender = new UserDto { Id = m.Sender.Id, UserName = m.Sender.UserName, Email = m.Sender.Email, FirstName = m.Sender.FirstName, LastName = m.Sender.LastName },
            Receiver = new UserDto { Id = m.Receiver.Id, UserName = m.Receiver.UserName, Email = m.Receiver.Email, FirstName = m.Receiver.FirstName, LastName = m.Receiver.LastName },
            Message = m.Message,
            Timestamp = m.Timestamp
        }).OrderByDescending(x=>x.Timestamp).ToList();

        return new Response<IReadOnlyList<ChatMessageDto>> { Result = dtos };
    }
}
