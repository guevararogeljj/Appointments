using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Mailing.Commands.SenderMailingsCommand;

public class SenderMailingsCommand : IRequest<Response<bool>>
{
    public List<MailingDto>? Mailing { get; set; }
}