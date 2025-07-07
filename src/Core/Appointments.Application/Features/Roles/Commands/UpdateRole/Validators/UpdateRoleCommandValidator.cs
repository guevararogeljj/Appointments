using FluentValidation;

namespace Appointments.Application.Features.Roles.Commands.UpdateRole.Validators;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
    }
}
