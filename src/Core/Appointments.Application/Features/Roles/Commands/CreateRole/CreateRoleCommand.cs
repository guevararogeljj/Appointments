using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommand : IRequest<Response<string>>
{
    public string Name { get; set; }
}
