using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleCommand : IRequest<Response<bool>>
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}
