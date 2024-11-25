using Domain.Errors;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Commands.RescheduleAppointment;

public class RescheduleAppointmentCommandValidator : AbstractValidator<RescheduleAppointmentCommand>
{
    public RescheduleAppointmentCommandValidator(IAppointmentRepository appointmentRepository)
    {
        RuleFor(command => command.AppointmentId)
            .NotEmpty()
                .WithErrorCode(AppointmentErrorCodes.InvalidAppointmentId)
                .WithMessage(AppointmentErrorMessages.InvalidAppointmentId);

        RuleFor(command => command.TimeSlot)
            .NotNull()
                .WithErrorCode(AppointmentErrorCodes.InvalidTimeSlot)
                .WithMessage(AppointmentErrorMessages.InvalidTimeSlot)
            .MustAsync(async (command, timeSlot, cancellationToken) =>
            {
                var timeSlots = await appointmentRepository.GetTimeSlotsByAvailabilityAsync(isAvailable: false, cancellationToken);
                if (timeSlots.Any(existingSlot => existingSlot
                    .ConflictsWith(command.TimeSlot.StartTime, command.TimeSlot.DurationInMinutes)))
                {
                    return false;
                }
                return true;
            })
                .WithErrorCode(AppointmentErrorCodes.InvalidTimeSlot)
                .WithMessage(AppointmentErrorMessages.CannotRescheduleAppointmentForBookedTimeSlot);
    }
}
