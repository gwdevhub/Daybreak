using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Daybreak.Behaviors;

public class ScaleFontWithSize : Behavior<TextBlock>
{
    public static readonly DependencyProperty MaxFontSizeProperty = DependencyProperty.Register("MaxFontSize", typeof(double), typeof(ScaleFontWithSize), new PropertyMetadata(12d));

    public double MaxFontSize
    {
        get
        {
            return (double)this.GetValue(MaxFontSizeProperty);
        }
        set
        {
            this.SetValue(MaxFontSizeProperty, value);
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        this.AssociatedObject.SizeChanged += (_, __) => this.CalculateFontSize();
        DependencyPropertyDescriptor.FromProperty(
            TextBlock.TextProperty, typeof(TextBlock)).AddValueChanged(this.AssociatedObject, (_, __) => this.CalculateFontSize());
        DependencyPropertyDescriptor.FromProperty(
            TextBlock.FontSizeProperty, typeof(TextBlock)).AddValueChanged(this.AssociatedObject, (_, __) => this.CalculateFontSize());
    }

    private void CalculateFontSize()
    {
        var textMeasurement = this.MeasureText(this.AssociatedObject.FontSize);
        var maximumTextMeasurement = this.MeasureText(this.MaxFontSize);
        var desiredWidthFontSize = this.MaxFontSize;
        var desiredHeightFontSize = this.MaxFontSize;

        if (Math.Round(textMeasurement.Height) != Math.Round(this.AssociatedObject.ActualHeight))
        {

            var scale = this.AssociatedObject.ActualHeight / maximumTextMeasurement.Height;
            desiredHeightFontSize = (this.MaxFontSize * scale) - 1;
            desiredHeightFontSize = desiredHeightFontSize <= this.MaxFontSize ? desiredHeightFontSize : this.MaxFontSize;
        }

        if (Math.Round(textMeasurement.Width) != Math.Round(this.AssociatedObject.ActualWidth))
        {
            var scale = this.AssociatedObject.ActualWidth / maximumTextMeasurement.Width;
            desiredWidthFontSize = (this.MaxFontSize * scale) - 1;
            desiredWidthFontSize = desiredWidthFontSize <= this.MaxFontSize ? desiredWidthFontSize : this.MaxFontSize;

        }

        var desiredFontSize = Math.Min(desiredHeightFontSize, desiredWidthFontSize);

        if ((int)desiredFontSize != (int)this.AssociatedObject.FontSize && desiredFontSize > 0)
        {
            this.AssociatedObject.FontSize = desiredFontSize;
            return;
        }
    }

    private Size MeasureText(double fontSize)
    {
        var formattedText = new FormattedText(this.AssociatedObject.Text, CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight,
            new Typeface(this.AssociatedObject.FontFamily, this.AssociatedObject.FontStyle, this.AssociatedObject.FontWeight, this.AssociatedObject.FontStretch),
            fontSize, Brushes.Black, VisualTreeHelper.GetDpi(this.AssociatedObject).PixelsPerDip);

        return new Size(formattedText.Width, formattedText.Height);
    }
}
