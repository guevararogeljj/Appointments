using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<Response<IReadOnlyList<UserDto>>>
{
}
