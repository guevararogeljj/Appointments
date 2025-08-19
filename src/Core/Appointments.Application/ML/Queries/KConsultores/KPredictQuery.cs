using MediatR;

namespace Appointments.Application.ML.Queries.KConsultores;

public record KPredictQuery(string ask) : IRequest<ChatbotOutput>;