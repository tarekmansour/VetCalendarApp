using Domain.Errors;
using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;

public sealed class TimeSlot
{
    public TimeSlotId Id { get; private set; } = default!;
    public DateTime StartTime { get; }
    public DateTime EndTime => StartTime.AddMinutes(DurationInMinutes);
    public int DurationInMinutes { get; }
    public bool IsAvailable { get; set; }

    private TimeSlot() { } //For EF core

    public TimeSlot(DateTime startTime, int durationInMinutes)
    {
        if (startTime <= DateTime.UtcNow)
            throw new ArgumentException(AppointmentErrorMessages.StartTimeSlotShouldBeValid, nameof(startTime));

        if (durationInMinutes <= 0)
            throw new ArgumentException(AppointmentErrorMessages.TimeSlotDurationShouldBeValid, nameof(durationInMinutes));

        StartTime = startTime;
        DurationInMinutes = durationInMinutes;
        IsAvailable = false;
    }

    public TimeSlot(TimeSlotId id, DateTime startTime, int durationInMinutes, bool isAvailable)
    {
        Id = id;
        StartTime = startTime;
        DurationInMinutes = durationInMinutes;
        IsAvailable = isAvailable;
    }

    public TimeSlot(TimeSlotId id, DateTime startTime, int durationInMinutes)
    {
        if (startTime <= DateTime.UtcNow)
            throw new ArgumentException(AppointmentErrorMessages.StartTimeSlotShouldBeValid, nameof(startTime));

        if (durationInMinutes <= 0)
            throw new ArgumentException(AppointmentErrorMessages.TimeSlotDurationShouldBeValid, nameof(durationInMinutes));

        Id = id;
        StartTime = startTime;
        DurationInMinutes = durationInMinutes;
        IsAvailable = false;
    }

    public bool ConflictsWith(DateTime newStartTime, int newDurationInMinutes)
    {
        var newEndTime = newStartTime.AddMinutes(newDurationInMinutes);
        return newStartTime < EndTime && newEndTime > StartTime;
    }

    public void Reserve()
    {
        IsAvailable = false;
    }

    public void Release()
    {
        IsAvailable = true;
    }
}
