using SharedKernel;

namespace Domain.ValueObjects.Identifiers;

public record ClientId : StrongTypedId
{
    public ClientId(int value) : base(value) { }
}