using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;
using Daybreak.Controls.Buttons;

namespace Daybreak.Controls;

/// <summary>
/// A drop‑down button that shows the current selection and lets the user pick another item.
/// </summary>
[TemplatePart(Name = PartMainButton, Type = typeof(HighlightButton))]
[TemplatePart(Name = PartArrowButton, Type = typeof(HighlightButton))]
public partial class DropDownButton : Control
{
    private const string PartMainButton = "PART_MainButton";
    private const string PartArrowButton = "PART_ArrowButton";
    private const string PartDropDown = "PART_DropDown";

    [GenerateDependencyProperty]
    private DataTemplate itemTemplate = default!;

    [GenerateDependencyProperty]
    private object selectedItem = default!;

    [GenerateDependencyProperty]
    private IEnumerable items = default!;

    [GenerateDependencyProperty(InitialValue = true)]
    private bool clickEnabled = true;

    [GenerateDependencyProperty]
    private Brush dropDownBackground = default!;

    [GenerateDependencyProperty]
    private Brush disableBrush = default!;

    public event EventHandler<object>? Clicked;

    public event EventHandler<object>? SelectionChanged;

    private HighlightButton? mainButton;
    private HighlightButton? arrowButton;

    static DropDownButton()
    {
        // connect to default style in Themes/Generic.xaml
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(DropDownButton),
            new FrameworkPropertyMetadata(typeof(DropDownButton)));
    }

    public DropDownButton()
    {
        // ignore right‑clicks so the default context‑menu logic does not interfere
        this.PreviewMouseRightButtonDown += (_, e) => e.Handled = true;
        this.PreviewMouseRightButtonUp += (_, e) => e.Handled = true;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        if (this.mainButton is not null)
        {
            this.mainButton.Clicked -= this.MainButton_Clicked;
        }

        if (this.arrowButton is not null)
        {
            this.arrowButton.Clicked -= this.ArrowButton_Clicked;
        }

        if (this.ContextMenu is not null)
        {
            this.ContextMenu.Opened -= this.ContextMenu_Opened;
        }

        this.mainButton = this.GetTemplateChild(PartMainButton) as HighlightButton;
        this.arrowButton = this.GetTemplateChild(PartArrowButton) as HighlightButton;

        if (this.mainButton is not null)
        {
            this.mainButton.Clicked += this.MainButton_Clicked;
        }

        if (this.arrowButton is not null)
        {
            this.arrowButton.Clicked += this.ArrowButton_Clicked;
        }

        if (this.ContextMenu is not null)
        {
            this.ContextMenu.Opened += this.ContextMenu_Opened;
        }
    }

    private void MainButton_Clicked(object? sender, object e)
    {
        this.Clicked?.Invoke(this, this.SelectedItem);
    }

    private void ArrowButton_Clicked(object? sender, object e)
    {
        if (this.ContextMenu is not null)
        {
            this.ContextMenu.PlacementTarget = this;
            this.ContextMenu.IsOpen = true;
        }
    }

    // When the context‑menu opens, locate the DropDownButtonContextMenu inside it and hook its CLR event.
    private void ContextMenu_Opened(object? sender, RoutedEventArgs e)
    {
        if (sender is not ContextMenu menu)
            return;

        if (menu.Template.FindName(PartDropDown, menu) is DropDownButtonContextMenu dropDown)
        {
            dropDown.ItemClicked -= this.DropDownButtonContextMenu_ItemClicked; // avoid duplicates
            dropDown.ItemClicked += this.DropDownButtonContextMenu_ItemClicked;
        }
    }

    private void DropDownButtonContextMenu_ItemClicked(object? _, object e)
    {
        this.SelectedItem = e;
        if (this.ContextMenu is not null)
        {
            this.ContextMenu.IsOpen = false;
        }

        this.SelectionChanged?.Invoke(this, e);
    }
}
