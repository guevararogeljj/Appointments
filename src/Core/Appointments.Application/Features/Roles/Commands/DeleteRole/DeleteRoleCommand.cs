using MediatR;

namespace Appointments.Application.Features.Roles.Commands.DeleteRole;

public class DeleteRoleCommand : IRequest<bool>
{
    public string Id { get; set; }
}
