using MediatR;

namespace Appointments.Application.ML.Queries.Reto97;

public record Reto97PredictQuery(string Ask) : IRequest<ChatbotOutput>;