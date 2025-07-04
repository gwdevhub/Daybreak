﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;
using System.Windows.Media;

namespace Daybreak.Controls.Buttons;
/// <summary>
/// Interaction logic for HighlightButton.xaml
/// </summary>
public partial class HighlightButton : UserControl
{
    public event EventHandler? Clicked;

    [GenerateDependencyProperty]
    private ICommand click = default!;

    [GenerateDependencyProperty]
    private bool highlighted;
    [GenerateDependencyProperty]
    private Brush highlightBrush = default!;
    [GenerateDependencyProperty]
    private Brush disabledBrush = default!;
    [GenerateDependencyProperty(InitialValue = "")]
    private string title = string.Empty;
    [GenerateDependencyProperty(InitialValue = HorizontalAlignment.Left)]
    private HorizontalAlignment horizontalContentAlignment = HorizontalAlignment.Left;
    [GenerateDependencyProperty(InitialValue = VerticalAlignment.Top)]
    private VerticalAlignment verticalContentAlignment = VerticalAlignment.Top;
    [GenerateDependencyProperty(InitialValue = TextAlignment.Center)]
    private TextAlignment textAlignment = TextAlignment.Center;
    [GenerateDependencyProperty(InitialValue = TextWrapping.Wrap)]
    private TextWrapping textWrapping = TextWrapping.Wrap;
    [GenerateDependencyProperty]
    private Thickness textPadding = new(0);
    [GenerateDependencyProperty]
    private UIElement buttonContent = default!;

    public HighlightButton()
    {
        this.InitializeComponent();
    }

    private void Grid_MouseEnter(object sender, MouseEventArgs e)
    {
        this.Highlighted = true;
    }

    private void Grid_MouseLeave(object sender, MouseEventArgs e)
    {
        this.Highlighted = false;
    }

    private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        this.Clicked?.Invoke(this, e);
        if (this.Click?.CanExecute(e) is true)
        {
            this.Click?.Execute(e);
        }
    }
}
