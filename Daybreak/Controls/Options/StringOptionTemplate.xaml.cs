using Daybreak.Shared.Models.Options;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Options;
/// <summary>
/// Interaction logic for StringOptionTemplate.xaml
/// </summary>
public partial class StringOptionTemplate : UserControl
{
    private OptionProperty optionProperty = default!;

    [GenerateDependencyProperty]
    private string value = string.Empty;

    public StringOptionTemplate()
    {
        this.InitializeComponent();
        this.DataContextChanged += this.StringOptionTemplate_DataContextChanged;
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == ValueProperty &&
            e.NewValue is string newValue)
        {
            if (!this.IsChangeValid(newValue))
            {
                this.Value = e.OldValue as string;
            }
            else
            {
                this.optionProperty.Setter(newValue);
            }
        }

        base.OnPropertyChanged(e);
    }

    private void StringOptionTemplate_DataContextChanged(object _, DependencyPropertyChangedEventArgs e)
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
        if (this.optionProperty.Getter()?.ToString() is not string stringValue)
        {
            return;
        }

        this.Value = stringValue;
    }

    private bool IsChangeValid(string newValue)
    {
        return this.optionProperty.Validator.IsValid(newValue);
    }
}
