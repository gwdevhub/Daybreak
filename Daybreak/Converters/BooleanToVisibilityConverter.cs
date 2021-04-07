using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Daybreak.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Set to true if you want to show control when boolean value is true.
        /// Set to false if you want to hide/collapse control when value is true.
        /// </summary>
        public bool TriggerValue { get; set; } = true;
        /// <summary>
        /// Set to true if you just want to hide the control, else set to false if you want to collapse the control.
        /// </summary>
        public bool IsHidden { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetVisibility(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private object GetVisibility(object value)
        {
            if (!(value is bool))
                return DependencyProperty.UnsetValue;
            bool objValue = (bool)value;
            if ((objValue && TriggerValue && IsHidden) || (!objValue && !TriggerValue && IsHidden))
            {
                return Visibility.Hidden;
            }
            if ((objValue && TriggerValue && !IsHidden) || (!objValue && !TriggerValue && !IsHidden))
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }
    }
}
