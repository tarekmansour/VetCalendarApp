using Domain.Errors;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Commands.CancelAppointment;

public class CancelAppointmentCommandValidator : AbstractValidator<CancelAppointmentCommand>
{
    public CancelAppointmentCommandValidator(IAppointmentRepository appointmentRepository)
    {
        RuleFor(command => command.AppointmentId)
            .NotEmpty()
                .WithErrorCode(AppointmentErrorCodes.InvalidAppointmentId)
                .WithMessage(AppointmentErrorMessages.InvalidAppointmentId);
    }
}