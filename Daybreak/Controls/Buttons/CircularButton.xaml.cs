using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;
using System.Windows.Media;

namespace Daybreak.Controls.Buttons;
/// <summary>
/// Interaction logic for HighlightButton.xaml
/// </summary>
public partial class CircularButton : UserControl
{
    public event EventHandler? Clicked;

    [GenerateDependencyProperty]
    private ICommand click = default!;

    [GenerateDependencyProperty]
    private Brush highlight = default!;

    [GenerateDependencyProperty]
    private bool isHighlighted;

    [GenerateDependencyProperty]
    private FrameworkElement content = default!;

    public CircularButton()
    {
        this.InitializeComponent();
    }

    private void Ellipse_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.IsHighlighted = true;
    }

    private void Ellipse_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.IsHighlighted = false;
    }

    private void Ellipse_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.Clicked?.Invoke(this, e);
        if (this.Click?.CanExecute(e) is true)
        {
            this.Click?.Execute(e);
        }
    }
}
