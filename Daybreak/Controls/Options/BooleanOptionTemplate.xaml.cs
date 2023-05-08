using Daybreak.Models.Options;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Options;
/// <summary>
/// Interaction logic for BooleanOptionTemplate.xaml
/// </summary>
public partial class BooleanOptionTemplate : UserControl
{
    private OptionProperty optionProperty = default!;

    [GenerateDependencyProperty]
    private bool value;

    public BooleanOptionTemplate()
    {
        this.InitializeComponent();
        this.DataContextChanged += this.BooleanOptionTemplate_DataContextChanged;
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == ValueProperty &&
            e.NewValue is bool newValue)
        {
            if (!this.IsChangeValid(newValue))
            {
                this.Value = (bool)e.OldValue;
            }
            else
            {
                this.optionProperty.Setter(newValue);
            }
        }

        base.OnPropertyChanged(e);
    }

    private void BooleanOptionTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
        if (this.optionProperty.Getter() is not bool boolValue)
        {
            return;
        }

        this.Value = boolValue;
    }

    private bool IsChangeValid(bool newValue)
    {
        return this.optionProperty.Validator.IsValid(newValue);
    }
}
