using Appointments.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Appointments.Application.Contracts.Persistence;
using Appointments.Application.Events;
using Appointments.Domain.Common;

namespace Appointments.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Response<IReadOnlyList<UserDto>>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;

    public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    public async Task<Response<IReadOnlyList<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.ToListAsync();

        var dtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            UserName = u.UserName
        }).ToList();

        _eventPublisher.Publish(dtos);

        return new Response<IReadOnlyList<UserDto>> { Result = dtos };
    }
}
