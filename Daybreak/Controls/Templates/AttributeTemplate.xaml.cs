using Daybreak.Models.Builds;
using Daybreak.Services.BuildTemplates;
using System;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Templates;

/// <summary>
/// Interaction logic for AttributeTemplate.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
public partial class AttributeTemplate : UserControl
{
    public event EventHandler<AttributeEntry>? HelpClicked;
    public event EventHandler<AttributeEntry>? AttributeChanged;

    private IAttributePointCalculator? attributePointCalculator;

    [GenerateDependencyProperty(InitialValue = false)]
    private bool canAdd;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool canSubtract;
    [GenerateDependencyProperty]
    private int attributePoints;

    public AttributeTemplate()
    {
        this.InitializeComponent();
        this.DataContextChanged += this.AttributeTemplate_DataContextChanged;
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == AttributePointsProperty &&
            this.DataContext is AttributeEntry attributeEntry)
        {
            var remainingPoints = this.AttributePoints;
            var requiredPointsForNextLevel = this.attributePointCalculator?.GetPointsRequiredToIncreaseRank(attributeEntry.Points) ?? 0;
            if (remainingPoints < requiredPointsForNextLevel)
            {
                this.CanAdd = false;
            }
            else if (this.DataContext.As<AttributeEntry>().Points < 12)
            {
                this.CanAdd = true;
            }
        }
    }

    public void InitializeAttributeTemplate(
        IAttributePointCalculator attributePointCalculator)
    {
        this.attributePointCalculator = attributePointCalculator.ThrowIfNull();
    }

    private void AttributeTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is AttributeEntry attributeEntry)
        {
            if (attributeEntry.Points > 0)
            {
                this.CanSubtract = true;
            }

            if (attributeEntry.Points < 12)
            {
                this.CanAdd = true;
            }
        }
    }

    private void MinusButton_Clicked(object sender, System.EventArgs e)
    {
        if (this.DataContext.As<AttributeEntry>().Points > 0)
        {
            this.DataContext.As<AttributeEntry>().Points--;
            this.CanSubtract = this.DataContext.As<AttributeEntry>().Points > 0;
            this.CanAdd = true;
            this.AttributeChanged?.Invoke(this, this.DataContext.As<AttributeEntry>());
        }
    }

    private void AddButton_Clicked(object sender, System.EventArgs e)
    {
        if (this.DataContext.As<AttributeEntry>().Points < 12)
        {
            this.DataContext.As<AttributeEntry>().Points++;
            this.CanAdd = this.DataContext.As<AttributeEntry>().Points < 12;
            this.CanSubtract = true;
            this.AttributeChanged?.Invoke(this, this.DataContext.As<AttributeEntry>());
        }
    }

    private void HelpButton_Clicked(object sender, System.EventArgs e)
    {
        if (this.DataContext is AttributeEntry attributeEntry)
        {
            this.HelpClicked?.Invoke(this, attributeEntry);
        }
    }
}
