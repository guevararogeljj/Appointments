using Appointments.Domain.Entities.Chat;

namespace Appointments.Application.Contracts.Persistence;

public interface IChatRoomRepository
{
    Task AddAsync(ChatRoom entity);
    Task<ChatRoom> GetByIdAsync(Guid id);
    Task<ChatRoom> GetChatRoomByUsersAsync(string user1Id, string user2Id);
    Task<IReadOnlyList<ChatRoom>> GetUserChatRoomsAsync(string userId);
}
