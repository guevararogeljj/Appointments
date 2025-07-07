using FluentValidation;

namespace Appointments.Application.Features.Appointments.Commands.CreateAppointment.Validators;

public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentCommandValidator()
    {
        RuleFor(a => a.PatientId)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(a => a.AppointmentDate)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .GreaterThan(DateTime.Now).WithMessage("{PropertyName} must be in the future.");

        RuleFor(a => a.Description)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");
    }
}
