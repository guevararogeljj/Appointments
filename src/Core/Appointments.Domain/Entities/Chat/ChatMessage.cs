using Appointments.Domain.Entities.Identity;

namespace Appointments.Domain.Entities.Chat;

public class ChatMessage : BaseEntity
{
    public Guid ChatRoomId { get; set; }
    public ChatRoom? ChatRoom { get; set; }

    public string? SenderId { get; set; }
    public ApplicationUser? Sender { get; set; }

    public string? ReceiverId { get; set; }
    public ApplicationUser? Receiver { get; set; }

    public string? Message { get; set; }
    public DateTime Timestamp { get; set; }
}
