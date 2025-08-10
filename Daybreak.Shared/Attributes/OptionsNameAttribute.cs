namespace Daybreak.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class OptionsNameAttribute : Attribute
{
    public string? Name { get; init; }
}
