using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Bots.BotDefault.Queries;

public class BotDefaultQuery : IRequest<Response<BotMessagesDto>>
{
    public string Message { get; set; }

    public BotDefaultQuery(string message)
    {
        Message = message;
    }
    public BotDefaultQuery()
    {
    }
}