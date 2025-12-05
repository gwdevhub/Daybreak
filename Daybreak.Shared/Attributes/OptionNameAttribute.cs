namespace Daybreak.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionNameAttribute : Attribute
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
