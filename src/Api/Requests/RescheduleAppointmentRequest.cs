using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record RescheduleAppointmentRequest
{
    [Required]
    public required DateTime StartTime { get; init; }
    [Required]
    public required int DurationInMinutes { get; init; }
};

