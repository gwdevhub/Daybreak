using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Daybreak.Shared.Converters;
public sealed class NullToUnsetValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value ?? DependencyProperty.UnsetValue;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}
