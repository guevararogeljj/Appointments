using MediatR;

namespace Appointments.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<IReadOnlyList<UserDto>>
{
}
