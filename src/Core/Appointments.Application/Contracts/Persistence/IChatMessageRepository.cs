using Appointments.Domain.Entities.Chat;

namespace Appointments.Application.Contracts.Persistence;

public interface IChatMessageRepository
{
    Task AddAsync(ChatMessage entity);
    Task<IReadOnlyList<ChatMessage>> GetMessagesByChatRoomIdAsync(Guid chatRoomId);
}
