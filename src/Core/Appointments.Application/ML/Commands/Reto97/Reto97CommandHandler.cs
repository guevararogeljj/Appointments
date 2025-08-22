using Appointments.Application.Services;
using MediatR;

namespace Appointments.Application.ML.Commands.Reto97;

public class Reto97CommandHandler : IRequestHandler<Reto97Command>
{
    private readonly IChatbotService _chatbotService;

    public Reto97CommandHandler(IChatbotService chatbotService)
    {
        _chatbotService = chatbotService;
    }

    public Task Handle(Reto97Command request, CancellationToken cancellationToken)
    {
        return _chatbotService.Train(request.TrainingDataPath);
    }
}