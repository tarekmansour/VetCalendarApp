using Domain.ValueObjects.Identifiers;
using System.ComponentModel.DataAnnotations;

namespace Api.Requests;
public record BookAppointmentRequest
{
    [Required]
    public required PatientId PatientId { get; init; }
    [Required]
    public required DateTime StartTime { get; init; }
    [Required]
    public required int DurationInMinutes { get; init; }
};

