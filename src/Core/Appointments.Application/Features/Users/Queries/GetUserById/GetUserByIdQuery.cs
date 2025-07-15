using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<Response<UserDto>>
{
    public string? Id { get; set; }
}
