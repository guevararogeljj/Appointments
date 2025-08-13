using MediatR;

namespace Appointments.Application.Features.Messaging.Commands
{
    public record SendMessageCommand(string Message) : IRequest;
}

