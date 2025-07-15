using Appointments.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Common;

namespace Appointments.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Response<RoleDto>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public GetRoleByIdQueryHandler(RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork)
    {
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<RoleDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id);
        if (role == null)
        {
            return new Response<RoleDto> { Error = new Error("NotFound", $"Role with ID {request.Id} not found.") };
        }

        var dto = new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        };

        return new Response<RoleDto> { Result = dto };
    }
}
