using Appointments.Application.Services;
using MediatR;
using Microsoft.ApplicationInsights;

namespace Appointments.Application.ML.Queries.KConsultores;

public class KPredictQueryHandler : IRequestHandler<KPredictQuery, ChatbotOutput>
{
    private readonly IChatbotService _chatbotService;
    private readonly TelemetryClient _telemetryClient;
    
    public KPredictQueryHandler(IChatbotService chatbotService, TelemetryClient telemetryClient)
    {
        _chatbotService = chatbotService;
        _telemetryClient = telemetryClient;
    }
    
    public async Task<ChatbotOutput> Handle(KPredictQuery request, CancellationToken cancellationToken)
    {
        var response = new ChatbotOutput
        {
            PredictedLabel = string.Empty,
        };
        try
        {
            var result = await _chatbotService.GetAnswer(request.ask);
            if (result == null)
            {
                response.PredictedLabel = "Lo siento, no tengo una respuesta para esa pregunta.";
                return response;
            }

            response.PredictedLabel = result;
        }
        catch (Exception ex)
        {
            // Log the exception (not shown here for brevity)
            response.PredictedLabel = "Ocurrió un error al procesar la solicitud.";
        }

        return response;
    }
}