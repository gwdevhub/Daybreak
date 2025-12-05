namespace Daybreak.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public abstract class OptionValuesFactoryAttribute : Attribute
{
    public abstract List<object> ValuesFactory();
}
