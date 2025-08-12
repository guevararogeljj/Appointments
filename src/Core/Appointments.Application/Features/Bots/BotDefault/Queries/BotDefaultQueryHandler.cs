using Appointments.Domain.Common;
using MediatR;
using Microsoft.ApplicationInsights;

namespace Appointments.Application.Features.Bots.BotDefault.Queries;

public class BotDefaultQueryHandler : IRequestHandler<BotDefaultQuery, Response<BotMessagesDto>>
{
    private readonly IBotService _botService;
    private readonly TelemetryClient _telemetryClient;

    public BotDefaultQueryHandler(IBotService botService, TelemetryClient telemetryClient)
    {
        _botService = botService;
        _telemetryClient = telemetryClient;
    }
    public async Task<Response<BotMessagesDto>> Handle(BotDefaultQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<BotMessagesDto>();
        try
        {
            var botMessages = await _botService.GetConversation(request.Message);
            if (botMessages == null)
            {
                response.Error = new Error(
                    "NotFound",
                    "No conversation found for the provided message.");
                return response;
            }

            response = botMessages;
        }
        catch (Exception ex)
        {
            response.Error = new Error(
                "Error",
                "An error occurred while processing the request.");
            return response;
        }

        return response;
    }
}