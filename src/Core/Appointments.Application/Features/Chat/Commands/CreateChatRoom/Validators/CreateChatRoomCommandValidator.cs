using FluentValidation;

namespace Appointments.Application.Features.Chat.Commands.CreateChatRoom.Validators;

public class CreateChatRoomCommandValidator : AbstractValidator<CreateChatRoomCommand>
{
    public CreateChatRoomCommandValidator()
    {
        RuleFor(x => x.User1Id)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.User2Id)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}
