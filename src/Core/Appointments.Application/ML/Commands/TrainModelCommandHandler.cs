using Appointments.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Appointments.Application.ML.Commands
{
    public class TrainModelCommandHandler : IRequestHandler<TrainModelCommand>
    {
        private readonly IMlService _mlService;

        public TrainModelCommandHandler(IMlService mlService)
        {
            _mlService = mlService;
        }

        public Task Handle(TrainModelCommand request, CancellationToken cancellationToken)
        {
            _mlService.TrainModel(request.TrainingDataPath, request.ModelPath);
            return Task.CompletedTask;
        }
    }
}

