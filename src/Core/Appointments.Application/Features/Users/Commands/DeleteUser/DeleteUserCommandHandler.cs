using System.Security.Claims;
using Appointments.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Common;

namespace Appointments.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            return new Response<bool> { Error = new Error("NotFound", $"User with ID {request.Id} not found.") };
        }

        var result = await _userManager.DeleteAsync(user);
        var userRequest = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _unitOfWork.CompleteAsync(userRequest);
        return new Response<bool> { Result = result.Succeeded };
    }
}
