using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for ResourceBar.xaml
/// </summary>
public partial class VerticalResourceBar : UserControl
{
    [GenerateDependencyProperty]
    private double currentResourceValue;
    [GenerateDependencyProperty]
    private double maxResourceValue;
    [GenerateDependencyProperty]
    private Brush barColor;
    [GenerateDependencyProperty]
    private string text;
    [GenerateDependencyProperty(InitialValue = VerticalAlignment.Bottom)]
    private VerticalAlignment fillAlignment;

    public VerticalResourceBar()
    {
        this.InitializeComponent();
        this.MaxResourceValue = 1;
        this.CurrentResourceValue = 0;
        this.barColor = Brushes.Transparent;
        this.text = string.Empty;
    }
}
