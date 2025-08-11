using FluentValidation;

namespace Appointments.Application.Features.Mailing.Commands.SenderMailingsCommand;

public class SenderMailingsCommandValidator : AbstractValidator<SenderMailingsCommand>
{
    public SenderMailingsCommandValidator()
    {
        RuleFor(x => x.Mailing)
            .NotNull().WithMessage("Mailing data is required.");
        
        RuleForEach(x => x.Mailing)
            .NotNull().WithMessage("Each mailing entry must not be null.")
            .WithMessage("Each mailing entry must be valid.");
    }
}