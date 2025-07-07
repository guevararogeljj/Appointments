using FluentValidation;

namespace Appointments.Application.Features.Chat.Commands.SendMessage.Validators;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.ChatRoomId)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.ReceiverId)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");
    }
}
