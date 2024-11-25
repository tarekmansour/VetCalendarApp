using SharedKernel;

namespace Domain.ValueObjects.Identifiers;
public record PatientId : StrongTypedId
{
    public PatientId(int value) : base(value) { }
}