namespace Daybreak.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionRangeAttribute<T> : Attribute
    where T : IComparable<T>
{
    public T MinValue { get; init; } = default!;
    public T MaxValue { get; init; } = default!;
}
