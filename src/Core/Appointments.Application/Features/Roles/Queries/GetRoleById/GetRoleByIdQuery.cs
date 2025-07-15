using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdQuery : IRequest<Response<RoleDto>>
{
    public string Id { get; set; }
}
