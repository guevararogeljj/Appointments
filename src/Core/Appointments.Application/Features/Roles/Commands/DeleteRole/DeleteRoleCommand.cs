using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Roles.Commands.DeleteRole;

public class DeleteRoleCommand : IRequest<Response<bool>>
{
    public string Id { get; set; }
}
