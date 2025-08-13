using Appointments.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Appointments.Application.Features.Messaging.Commands
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand>
    {
        private readonly IMessageSender _messageSender;

        public SendMessageCommandHandler(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            await _messageSender.SendMessageAsync(request.Message);
        }
    }
}

