using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Entities.Chat;
using Appointments.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly AppointmentsDbContext _context;

    public ChatMessageRepository(AppointmentsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ChatMessage entity)
    {
        await _context.ChatMessages.AddAsync(entity);
    }

    public async Task<IReadOnlyList<ChatMessage>> GetMessagesByChatRoomIdAsync(Guid chatRoomId)
    {
        return await _context.ChatMessages
            .Where(cm => cm.ChatRoomId == chatRoomId)
            .OrderBy(cm => cm.Timestamp)
            .Include(cm => cm.Sender)
            .Include(cm => cm.Receiver)
            .ToListAsync();
    }
}
