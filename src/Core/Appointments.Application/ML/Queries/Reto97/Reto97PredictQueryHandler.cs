using Appointments.Application.Services;
using MediatR;

namespace Appointments.Application.ML.Queries.Reto97;

public class Reto97PredictQueryHandler : IRequestHandler<Reto97PredictQuery, ChatbotOutput>
{
    
    private readonly IChatbotService _chatbotService;
    public Reto97PredictQueryHandler(IChatbotService chatbotService)
    {
        _chatbotService = chatbotService;
    }
    
    public async Task<ChatbotOutput> Handle(Reto97PredictQuery request, CancellationToken cancellationToken)
    {
        var response = new ChatbotOutput
        {
            PredictedLabel = string.Empty,
        };
        try
        {
            var result = await _chatbotService.GetAnswerReto97(request.Ask);
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