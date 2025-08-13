using MediatR;

namespace Appointments.Application.ML.Commands
{
    public record TrainModelCommand(string TrainingDataPath, string ModelPath) : IRequest;
}

