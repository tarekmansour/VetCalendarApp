using Domain.Errors;
using FluentValidation;

namespace Application.Commands.CreateClient;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
                .WithErrorCode(ClientErrorCodes.InvalidClientName)
                .WithMessage(ClientErrorMessages.ClientNameShouldNotBeEmpty);

        RuleFor(command => command.PhoneNumber)
            .NotEmpty()
                .WithErrorCode(ClientErrorCodes.InvalidClientPhoneNumber)
                .WithMessage(ClientErrorMessages.ClientPhoneNumberShouldNotBeEmpty);

        RuleFor(command => command.Email)
            .NotEmpty()
                .WithErrorCode(ClientErrorCodes.InvalidClientEmail)
                .WithMessage(ClientErrorMessages.ClientEmailShouldNotBeEmpty);
    }
}
