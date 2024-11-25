namespace SharedKernel;

public abstract record StrongTypedId
{
    public int Value { get; }

    protected StrongTypedId(int value)
    {
        if (value <= 0)
            throw new ArgumentException("ID must be greater than zero.", nameof(value));
        Value = value;
    }
}
