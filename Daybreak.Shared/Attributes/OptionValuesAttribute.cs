namespace Daybreak.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class OptionValuesAttribute : Attribute
{
    public required object[] Values { get; init; }
}
