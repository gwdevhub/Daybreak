using Daybreak.Shared.Models.Options;
using System;
using System.Collections.ObjectModel;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Options;
/// <summary>
/// Interaction logic for EnumOptionTemplate.xaml
/// </summary>
public partial class EnumOptionTemplate : UserControl
{
    private OptionProperty optionProperty = default!;

    public ObservableCollection<object> PossibleEnumValues { get; } = [];

    [GenerateDependencyProperty]
    private object value = default!;

    public EnumOptionTemplate()
    {
        this.InitializeComponent();
        this.DataContextChanged += this.EnumOptionTemplate_DataContextChanged;
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

    private void EnumOptionTemplate_DataContextChanged(object _, DependencyPropertyChangedEventArgs e)
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
        this.PossibleEnumValues.ClearAnd().AddRange(Enum.GetValues(this.optionProperty.Type).OfType<object>());
        this.Value = this.optionProperty.Getter();
    }

    private bool IsChangeValid(object newValue)
    {
        return this.optionProperty.Validator.IsValid(newValue);
    }
}
