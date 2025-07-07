using MediatR;

namespace Appointments.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public string Id { get; set; }
}
