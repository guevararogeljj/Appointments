using System.Security.Claims;
using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Common;
using Appointments.Domain.Entities.Chat;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Appointments.Application.Features.Chat.Commands.SendMessage;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Response<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SendMessageCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<Guid>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
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
        var user = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _unitOfWork.CompleteAsync(user);

        return new Response<Guid> { Result = chatMessage.Id };
    }
}
