using Appointments.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Appointments.Application.Contracts.Persistence;

namespace Appointments.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, string>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork)
    {
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new ApplicationRole { Name = request.Name };
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            throw new ApplicationException($"Error creating role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        await _unitOfWork.CompleteAsync();
        return role.Id;
    }
}
