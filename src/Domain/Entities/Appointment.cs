using Domain.Errors;
using Domain.Exceptions;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;

public sealed class Appointment
{
    public AppointmentId Id { get; private set; } = default!;
    public PatientId PatientId { get; private set; } = default!;
    public TimeSlotId TimeSlotId { get; private set; } = default!;
    public AppointmentStatus Status { get; private set; } = default!;
    public TimeSlot TimeSlot { get; private set; } = default!;

    private Appointment() { } //For EF core
    public Appointment(PatientId patientId, TimeSlot timeSlot)
    {
        PatientId = patientId;
        TimeSlot = timeSlot;
        Status = AppointmentStatus.Scheduled;
    }
    public Appointment(AppointmentId id, PatientId patientId, TimeSlot timeSlot)
    {
        Id = id;
        PatientId = patientId;
        TimeSlot = timeSlot;
        Status = AppointmentStatus.Scheduled;
    }

    public void Cancel()
    {
        if (Status == AppointmentStatus.Cancelled)
            throw new AppointmentException(AppointmentErrorMessages.AlreadyCancelled);

        Status = AppointmentStatus.Cancelled;
        TimeSlot.Release();
    }

    public void Reschedule(TimeSlot newTimeSlot)
    {
        if (Status == AppointmentStatus.Cancelled)
            throw new AppointmentException(AppointmentErrorMessages.CannotRescheduleCancelled);

        TimeSlot = newTimeSlot;
        Status = AppointmentStatus.Rescheduled;

        TimeSlot.Reserve();
    }
}