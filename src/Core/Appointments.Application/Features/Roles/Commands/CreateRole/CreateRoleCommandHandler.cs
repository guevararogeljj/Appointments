using System.Security.Claims;
using Appointments.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Common;

namespace Appointments.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Response<string>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new ApplicationRole { Name = request.Name };
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            return new Response<string> { Error = new Error("BadRequest", $"Error creating role: {string.Join(", ", result.Errors.Select(e => e.Description))}") };
        }

        var user = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _unitOfWork.CompleteAsync(user);
        return new Response<string> { Result = role.Id };
    }
}
