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
public partial class HighlightButton : UserControl
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
    [GenerateDependencyProperty]
    private HorizontalAlignment horizontalContentAlignment = HorizontalAlignment.Left;
    [GenerateDependencyProperty]
    private VerticalAlignment verticalContentAlignment = VerticalAlignment.Top;

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
    }
}
