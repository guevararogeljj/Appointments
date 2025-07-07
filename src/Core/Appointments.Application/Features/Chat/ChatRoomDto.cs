using Appointments.Application.Features.Users;

namespace Appointments.Application.Features.Chat;

public class ChatRoomDto
{
    public Guid Id { get; set; }
    public UserDto User1 { get; set; }
    public UserDto User2 { get; set; }
}
