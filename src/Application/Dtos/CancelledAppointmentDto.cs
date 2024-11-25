using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;

namespace Application.Dtos;

public record CancelledAppointmentDto(
    AppointmentId Id,
    PatientId PatientId,
    DateTime StartTime,
    AppointmentStatus Status);
