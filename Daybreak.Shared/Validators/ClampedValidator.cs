using System.ComponentModel;

namespace Daybreak.Shared.Validators;

public sealed class ClampedValidator<T>(T minValue, T maxValue) : IValidator
    where T : IComparable<T>
{
    private readonly T minValue = minValue, maxValue = maxValue;

    public bool IsValid(object value)
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
