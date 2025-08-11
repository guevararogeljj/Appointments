using FluentValidation;

namespace Appointments.Application.Features.Mailing.Commands;

public class SenderMailingCommandValidator : AbstractValidator<SenderMailingCommand>
{
    public SenderMailingCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required.")
            .MaximumLength(100).WithMessage("Subject cannot exceed 100 characters.");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Body is required.")
            .MaximumLength(500).WithMessage("Body cannot exceed 500 characters.");
    }
}