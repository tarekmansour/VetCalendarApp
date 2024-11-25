using Domain.ValueObjects.Identifiers;
using Domain.ValueObjects;

namespace Application.Dtos;

public record RescheduledAppointmentDto(
    AppointmentId Id,
    PatientId PatientId,
    DateTime NewStartTime,
    AppointmentStatus Status);
