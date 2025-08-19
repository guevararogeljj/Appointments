using MediatR;

namespace Appointments.Application.ML.Commands.KConsultores;

public record KConsultoresCommand(string TrainingDataPath) : IRequest;