using MediatR;

namespace Appointments.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommand : IRequest<string>
{
    public string Name { get; set; }
}
