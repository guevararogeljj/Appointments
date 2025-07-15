using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Roles.Queries.GetAllRoles;

public class GetAllRolesQuery : IRequest<Response<IReadOnlyList<RoleDto>>>
{
}
