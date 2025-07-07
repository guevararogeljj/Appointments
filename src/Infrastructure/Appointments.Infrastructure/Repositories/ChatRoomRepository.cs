using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Entities.Chat;
using Appointments.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Repositories;

public class ChatRoomRepository : IChatRoomRepository
{
    private readonly AppointmentsDbContext _context;

    public ChatRoomRepository(AppointmentsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ChatRoom entity)
    {
        await _context.ChatRooms.AddAsync(entity);
    }

    public async Task<ChatRoom> GetByIdAsync(Guid id)
    {
        return await _context.ChatRooms
            .Include(cr => cr.User1)
            .Include(cr => cr.User2)
            .SingleOrDefaultAsync(cr => cr.Id == id);
    }

    public async Task<ChatRoom> GetChatRoomByUsersAsync(string user1Id, string user2Id)
    {
        return await _context.ChatRooms
            .Include(cr => cr.User1)
            .Include(cr => cr.User2)
            .SingleOrDefaultAsync(cr =>
                (cr.User1Id == user1Id && cr.User2Id == user2Id) ||
                (cr.User1Id == user2Id && cr.User2Id == user1Id));
    }

    public async Task<IReadOnlyList<ChatRoom>> GetUserChatRoomsAsync(string userId)
    {
        return await _context.ChatRooms
            .Include(cr => cr.User1)
            .Include(cr => cr.User2)
            .Where(cr => cr.User1Id == userId || cr.User2Id == userId)
            .ToListAsync();
    }
}
