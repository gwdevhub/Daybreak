using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for TileButton.xaml
/// </summary>
public partial class TileButton : UserControl
{
    public event EventHandler? Clicked;

    [GenerateDependencyProperty]
    public bool highlighted;
    [GenerateDependencyProperty(InitialValue = "")]
    public string title = string.Empty;
    [GenerateDependencyProperty]
    public FrameworkElement InnerContent = default!;
    [GenerateDependencyProperty]
    public Brush HighlightColor = default!;

    public TileButton()
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
