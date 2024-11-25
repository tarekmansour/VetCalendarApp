using Domain.Entities;
using Domain.ValueObjects.Identifiers;

namespace Domain.Interfaces;

public interface IAppointmentRepository
{
    Task<IEnumerable<TimeSlot>> GetTimeSlotsByAvailabilityAsync(bool isAvailable, CancellationToken cancellationToken = default);
    Task<AppointmentId> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task<bool> ExistsPatientAsync(PatientId patientId, CancellationToken cancellationToken = default);
    Task<Appointment?> GetAppointmentByIdAsync(AppointmentId appointmentId, CancellationToken cancellationToken = default);
}
