namespace Daybreak.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class OptionsNameAttribute : Attribute
{
    public string? Name { get; init; }
}
