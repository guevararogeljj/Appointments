using Appointments.Application.Features.Users;

namespace Appointments.Application.Features.Chat;

public class ChatMessageDto
{
    public Guid Id { get; set; }
    public Guid ChatRoomId { get; set; }
    public UserDto Sender { get; set; }
    public UserDto Receiver { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}
