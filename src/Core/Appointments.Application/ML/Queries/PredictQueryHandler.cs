using Appointments.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Appointments.Application.ML;

namespace Appointments.Application.ML.Queries
{
    public class PredictQueryHandler : IRequestHandler<PredictQuery, ModelOutput>
    {
        private readonly IMlService _mlService;

        public PredictQueryHandler(IMlService mlService)
        {
            _mlService = mlService;
        }

        public Task<ModelOutput> Handle(PredictQuery request, CancellationToken cancellationToken)
        {
            var result = _mlService.Predict(request.Input);
            return Task.FromResult(result);
        }
    }
}

