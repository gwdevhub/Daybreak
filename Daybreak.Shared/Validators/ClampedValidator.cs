using System.ComponentModel;

namespace Daybreak.Shared.Validators;

public sealed class ClampedValidator<T> : IValidator
    where T : IComparable<T>
{
    private readonly T minValue, maxValue;

    public ClampedValidator(T minValue, T maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

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
