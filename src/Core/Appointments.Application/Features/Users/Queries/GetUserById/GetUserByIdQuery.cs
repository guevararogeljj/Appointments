using MediatR;

namespace Appointments.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public string Id { get; set; }
}
