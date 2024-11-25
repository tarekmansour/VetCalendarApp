using SharedKernel;

namespace Domain.ValueObjects.Identifiers;

public record AppointmentId : StrongTypedId
{
    public AppointmentId(int value) : base(value) { }
}