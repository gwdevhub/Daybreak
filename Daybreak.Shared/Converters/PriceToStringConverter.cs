using System;
using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Converters;
public sealed class PriceToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            throw new InvalidOperationException($"Unable to convert to {targetType.Name}");
        }

        var price = value switch
        {
            byte byteVal => (double)byteVal,
            short shortVal => (double)shortVal,
            ushort ushortVal => (double)ushortVal,
            int intVal => (double)intVal,
            uint uintVal => (double)uintVal,
            long longVal => (double)longVal,
            ulong ulongVal => (double)ulongVal,
            decimal decimalVal => (double)decimalVal,
            float floatVal => (double)floatVal,
            double doubleVal => (double)doubleVal,
            _ => throw new InvalidOperationException($"Unable to convert from {value.GetType().Name}")
        };

        var suffix = 'g';
        if (price > 1000)
        {
            price /= 1000;
            suffix = 'k';
        }

        return $"{price}{suffix}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
