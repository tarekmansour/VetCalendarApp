namespace Domain.Errors;

public static class AppointmentErrorMessages
{
    public static readonly string InvalidAppointmentId = "AppointmentId must be a positive integer.";
    public static readonly string StartTimeSlotShouldBeValid = "Invalid appointment start time slot. Appointment slot cannot be in the past.";
    public static readonly string TimeSlotDurationShouldBeValid = "Invalid time slot duration. Duration must be greater than zero.";
    public static readonly string AlreadyCancelled = "Appointment is already cancelled.";
    public static readonly string CannotRescheduleCancelled = "Cannot reschedule a cancelled appointment.";
    public static readonly string CannotBookAppointmentForBookedTimeSlot = "Cannot book new appointment. The given time slot is already booked";
    public static readonly string InvalidTimeSlot = "Invalid time slot. Time slot should not be null.";
    public static readonly string InvalidPatientId = "Invalid patient Id. Patient Id should not be null.";
    public static readonly string NotFoundPatientToBookAppointment = "Invalid patient Id. Patient not found to book an appointment.";
    public static readonly string CannotRescheduleAppointmentForBookedTimeSlot = "Cannot reschedule appointment. The given time slot is already booked";

}
