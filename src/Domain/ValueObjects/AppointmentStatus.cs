namespace Domain.ValueObjects;

public record AppointmentStatus
{
    public static readonly AppointmentStatus Scheduled = new("Scheduled");
    public static readonly AppointmentStatus Cancelled = new("Cancelled");
    public static readonly AppointmentStatus Rescheduled = new("Rescheduled");
    public string Value { get; }

    private AppointmentStatus(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;
}

