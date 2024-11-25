using SharedKernel;

namespace Domain.ValueObjects.Identifiers;
public record TimeSlotId : StrongTypedId
{
    public TimeSlotId(int value) : base(value) { }
}