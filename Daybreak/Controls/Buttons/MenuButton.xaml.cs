using System;
using System.Windows.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace Daybreak.Controls.Buttons;

/// <summary>
/// Interaction logic for MenuButton.xaml
/// </summary>
public partial class MenuButton : UserControl
{
    public event EventHandler? Clicked;

    [GenerateDependencyProperty]
    private ICommand click = default!;

    [GenerateDependencyProperty]
    private bool highlighted;
    [GenerateDependencyProperty]
    private Brush highlightColor = default!;
    [GenerateDependencyProperty]
    private FrameworkElement innerContent = default!;
    [GenerateDependencyProperty(InitialValue = "")]
    private string title = string.Empty;
    [GenerateDependencyProperty]
    private HorizontalAlignment horizontalContentAlignment = HorizontalAlignment.Left;
    [GenerateDependencyProperty]
    private VerticalAlignment verticalContentAlignment = VerticalAlignment.Top;

    public MenuButton()
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
