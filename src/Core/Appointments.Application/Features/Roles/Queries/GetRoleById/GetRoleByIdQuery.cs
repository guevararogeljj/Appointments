using MediatR;

namespace Appointments.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdQuery : IRequest<RoleDto>
{
    public string Id { get; set; }
}
