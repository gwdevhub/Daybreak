using System;
using System.Windows.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Daybreak.Controls.Buttons;

/// <summary>
/// Interaction logic for MenuButton.xaml
/// </summary>
public partial class MenuButton : UserControl
{
    public event EventHandler? Clicked;

    [GenerateDependencyProperty]
    private bool highlighted;
    [GenerateDependencyProperty]
    private Brush highlightColor = default!;
    [GenerateDependencyProperty]
    private FrameworkElement innerContent = default!;
    [GenerateDependencyProperty(InitialValue = "")]
    private string title = string.Empty;

    public MenuButton()
    {
        this.InitializeComponent();
    }

    private void Grid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.Highlighted = true;
    }

    private void Grid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.Highlighted = false;
    }

    private void Rectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
}
