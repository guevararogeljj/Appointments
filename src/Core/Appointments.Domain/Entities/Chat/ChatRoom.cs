using Appointments.Domain.Entities.Identity;

namespace Appointments.Domain.Entities.Chat;

public class ChatRoom : BaseEntity
{
    public string User1Id { get; set; }
    public ApplicationUser User1 { get; set; }

    public string User2Id { get; set; }
    public ApplicationUser User2 { get; set; }

    public ICollection<ChatMessage> Messages { get; set; }
}
