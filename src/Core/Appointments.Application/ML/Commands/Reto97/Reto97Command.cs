using MediatR;

namespace Appointments.Application.ML.Commands.Reto97;

public record Reto97Command(string TrainingDataPath) : IRequest;