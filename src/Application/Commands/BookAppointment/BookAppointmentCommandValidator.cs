using Domain.Errors;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Commands.BookAppointment;

public class BookAppointmentCommandValidator : AbstractValidator<BookAppointmentCommand>
{
    public BookAppointmentCommandValidator(IAppointmentRepository appointmentRepository)
    {
        RuleFor(command => command.PatientId)
            .NotEmpty()
                .WithErrorCode(AppointmentErrorCodes.InvalidPatientId)
                .WithMessage(AppointmentErrorMessages.InvalidPatientId)
            .MustAsync(async (command, patientId, cancellationToken) =>
            {
                return await appointmentRepository.ExistsPatientAsync(command.PatientId, cancellationToken);
            })
                .WithErrorCode(AppointmentErrorCodes.InvalidPatientId)
                .WithMessage(AppointmentErrorMessages.NotFoundPatientToBookAppointment);       

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
                .WithMessage(AppointmentErrorMessages.CannotBookAppointmentForBookedTimeSlot);
    }
}
