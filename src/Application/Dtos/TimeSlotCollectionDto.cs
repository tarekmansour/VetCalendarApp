namespace Application.Dtos;

public record TimeSlotCollectionDto(
    int count = 0,
    IEnumerable<TimeSlotDto>? Items = null);
