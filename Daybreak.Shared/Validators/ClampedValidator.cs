using System.ComponentModel;

namespace Daybreak.Shared.Validators;

public abstract class ClampedValidator(object minValue, object maxValue) : IValidator
{
    public object MinValue { get; } = minValue;
    public object MaxValue { get; } = maxValue;

    public abstract bool IsValid(object value);
}

public sealed class ClampedValidator<T>(T minValue, T maxValue) : ClampedValidator(minValue, maxValue)
    where T : IComparable<T>
{
    private readonly T minValue = minValue, maxValue = maxValue;

    public override bool IsValid(object value)
    {
        T numberValue;
        if (value is string stringValue)
        {
            var tc = TypeDescriptor.GetConverter(typeof(T));
            numberValue = (T)tc.ConvertFrom(stringValue)!;
        }
        else if (value is T typedValue)
        {
            numberValue = typedValue;
        }
        else
        {
            return false;
        }

        if (numberValue is null)
        {
            return false;
        }

        if (numberValue.CompareTo(this.minValue) < 0)
        {
            return false;
        }

        if (numberValue.CompareTo(this.maxValue) > 0)
        {
            return false;
        }

        return true;
    }
}
