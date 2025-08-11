using Appointments.Domain.Common;

namespace Appointments.Application.Features.Bots.BotDefault.Queries;

public interface IBotService
{
    Task <Response<BotMessagesDto>> GetConversation(string message);
}