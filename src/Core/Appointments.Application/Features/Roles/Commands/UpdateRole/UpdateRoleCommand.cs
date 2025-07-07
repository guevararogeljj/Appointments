using MediatR;

namespace Appointments.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleCommand : IRequest<bool>
{
    public string Id { get; set; }
    public string Name { get; set; }
}
