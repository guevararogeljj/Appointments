using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Mailing.Commands;

public class SenderMailingCommand : IRequest<Response<bool>>
{
    public string? Email { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
}