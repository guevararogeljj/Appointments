using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Entities.Chat;
using MediatR;

namespace Appointments.Application.Features.Chat.Commands.SendMessage;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public SendMessageCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var chatMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ChatRoomId = request.ChatRoomId,
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Message = request.Message,
            Timestamp = DateTime.UtcNow
        };

        await _unitOfWork.ChatMessages.AddAsync(chatMessage);
        await _unitOfWork.CompleteAsync();

        return chatMessage.Id;
    }
}
