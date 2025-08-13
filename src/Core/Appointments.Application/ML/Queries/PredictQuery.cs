using Appointments.Application.ML;
using MediatR;

namespace Appointments.Application.ML.Queries
{
    public record PredictQuery(ModelInput Input) : IRequest<ModelOutput>;
}

