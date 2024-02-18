using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Extensions;

namespace Daybreak.Launch;
/// <summary>
/// Interaction logic for MinimapWindow.xaml
/// </summary>
public partial class MinimapWindow : MetroWindow
{
    [GenerateDependencyProperty]
    private bool pinned = false;
    [GenerateDependencyProperty]
    private FrameworkElement content = default!;

    public MinimapWindow()
    {
        this.InitializeComponent();
    }

    private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ChangedButton is System.Windows.Input.MouseButton.Left)
        {
            e.Handled = true;
            this.DragMove();
        }
    }

    private void HighlightButton_Clicked(object sender, System.EventArgs e)
    {
        this.Pinned = !this.Pinned;
    }
}
