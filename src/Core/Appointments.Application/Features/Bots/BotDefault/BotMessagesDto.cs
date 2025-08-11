namespace Appointments.Application.Features.Bots.BotDefault;

public class BotMessagesDto
{
    public Guid Id { get; set; }
    public String? Message { get; set; }
    public DateTime Timestamp { get; set; }
    public string? SenderName { get; set; }
    public string? ReceiverName { get; set; }
    public bool IsSenderBot { get; set; }
}