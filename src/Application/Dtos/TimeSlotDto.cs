using Domain.ValueObjects.Identifiers;

namespace Application.Dtos;

public record TimeSlotDto(
    TimeSlotId Id,
    DateTime StartTime,
    DateTime EndTime,
    bool IsAvailable);
