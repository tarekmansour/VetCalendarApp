using Domain.Errors;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Commands.CreatePatient;

public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientCommandValidator(IClientRepository clientRepository)
    {
        RuleFor(command => command.ClientId)
            .NotNull()
                .WithErrorCode(ClientErrorCodes.InvalidClientId)
                .WithMessage(ClientErrorMessages.ClientIdShouldNotBeEmpty)
            .MustAsync(async (command, patientId, cancellationToken) =>
            {
                return await clientRepository.ExistsClientAsync(command.ClientId, cancellationToken);
            })
                .WithErrorCode(ClientErrorCodes.InvalidClientId)
                .WithMessage(ClientErrorMessages.NotFoundClientToAttachClient);

        RuleFor(command => command.Name)
            .NotEmpty()
                .WithErrorCode(ClientErrorCodes.InvalidPatientName)
                .WithMessage(ClientErrorMessages.PatientNameShouldNotBeEmpty);

        RuleFor(command => command.Species)
            .NotEmpty()
                .WithErrorCode(ClientErrorCodes.InvalidPatientSpecies)
                .WithMessage(ClientErrorMessages.PatientSpeciesShouldNotBeEmpty);

        RuleFor(command => command.DateOfBirth)
            .NotEmpty()
                .WithErrorCode(ClientErrorCodes.InvalidPatientDateOfBirth)
                .WithMessage(ClientErrorMessages.PatientDateOfBirthShouldNotBeEmpty)
            .LessThanOrEqualTo(DateTime.Now)
                .WithErrorCode(ClientErrorCodes.InvalidPatientDateOfBirth)
                .WithMessage(ClientErrorMessages.PatientDateOfBirthShouldBeInThePast);
    }
}
