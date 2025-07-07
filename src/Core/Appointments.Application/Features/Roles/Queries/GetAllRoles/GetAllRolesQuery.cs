using MediatR;

namespace Appointments.Application.Features.Roles.Queries.GetAllRoles;

public class GetAllRolesQuery : IRequest<IReadOnlyList<RoleDto>>
{
}
