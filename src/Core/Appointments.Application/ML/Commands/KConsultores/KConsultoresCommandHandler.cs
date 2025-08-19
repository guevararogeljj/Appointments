using Appointments.Application.Services;
using MediatR;

namespace Appointments.Application.ML.Commands.KConsultores;

public class KConsultoresCommandHandler : IRequestHandler<KConsultoresCommand>
{
    private readonly IChatbotService _chatbotService;

    public KConsultoresCommandHandler(IChatbotService chatbotService)
    {
        _chatbotService = chatbotService;
    }
  
    public Task Handle(KConsultoresCommand request, CancellationToken cancellationToken)
    {
        return _chatbotService.Train(request.TrainingDataPath);
    }
}