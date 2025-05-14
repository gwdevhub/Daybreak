using Daybreak.Shared.Models.Options;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Options;
/// <summary>
/// Interaction logic for PrimitiveRangeOptionTemplate.xaml
/// </summary>
public partial class PrimitiveRangeOptionTemplate : UserControl
{
    private OptionProperty optionProperty = default!;

    [GenerateDependencyProperty]
    private object value = default!;

    [GenerateDependencyProperty]
    private object minValue = default!;

    [GenerateDependencyProperty]
    private object maxValue = default!;

    public PrimitiveRangeOptionTemplate(
        object minValue, object maxValue)
    {
        this.InitializeComponent();
        this.DataContextChanged += this.PrimitiveRangeOptionTemplate_DataContextChanged;
        this.MinValue = minValue;
        this.MaxValue = maxValue;
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == ValueProperty)
        {
            if (!this.IsChangeValid(e.NewValue))
            {
                this.Value = e.OldValue;
            }
            else
            {
                this.optionProperty.Setter(e.NewValue);
            }
        }

        base.OnPropertyChanged(e);
    }

    private void PrimitiveRangeOptionTemplate_DataContextChanged(object _, DependencyPropertyChangedEventArgs e)
    {
        if (e.Property != DataContextProperty)
        {
            return;
        }

        if (e.NewValue is not OptionProperty optionProperty)
        {
            return;
        }

        this.optionProperty = optionProperty;
        this.Value = this.optionProperty.Getter();
    }

    private bool IsChangeValid(object newValue)
    {
        return this.optionProperty.Validator.IsValid(newValue);
    }
}
