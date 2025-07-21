using System.Security.Claims;
using Appointments.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Common;

namespace Appointments.Application.Features.Roles.Commands.DeleteRole;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Response<bool>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteRoleCommandHandler(RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id);
        if (role == null)
        {
            return new Response<bool> { Error = new Error("NotFound", $"Role with ID {request.Id} not found.") };
        }

        var result = await _roleManager.DeleteAsync(role);
        var user = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _unitOfWork.CompleteAsync(user);
        return new Response<bool> { Result = result.Succeeded };
    }
}
