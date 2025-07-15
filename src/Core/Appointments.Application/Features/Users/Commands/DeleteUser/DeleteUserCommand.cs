using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Response<bool>>
{
    public string? Id { get; set; }
}
