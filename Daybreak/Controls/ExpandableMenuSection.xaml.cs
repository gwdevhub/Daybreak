using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media.Animation;

namespace Daybreak.Controls;
/// <summary>
/// Interaction logic for ExpandableMenuSection.xaml
/// </summary>
public partial class ExpandableMenuSection : UserControl
{
    [GenerateDependencyProperty]
    private string sectionTitle = string.Empty;
    [GenerateDependencyProperty]
    private double expanderHeight = 0;
    [GenerateDependencyProperty(InitialValue = true)]
    private bool expanderButtonEnabled;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool expanded = false;

    public ObservableCollection<FrameworkElement> Children { get; } = [];

    public ExpandableMenuSection()
    {
        this.InitializeComponent();
    }

    private void MenuButton_Clicked(object sender, EventArgs e)
    {
        this.ExpanderButtonEnabled = false;
        var desiredHeight = 0d;
        foreach (var child in this.Children)
        {
            desiredHeight += child.DesiredSize.Height > 0 ?
                child.DesiredSize.Height :
                double.IsNaN(child.Height) ?
                    0 :
                    child.Height;
        }

        var doubleAnimation = new DoubleAnimation();
        
        if (this.expanded)
        {
            doubleAnimation.From = desiredHeight;
            doubleAnimation.To = 0;
        }
        else
        {
            doubleAnimation.From = 0;
            doubleAnimation.To = desiredHeight;
        }

        doubleAnimation.AccelerationRatio = 0.3;
        doubleAnimation.DecelerationRatio = 0.3;
        doubleAnimation.Duration = TimeSpan.FromMilliseconds(100);
        doubleAnimation.Completed += (_, _) =>
        {
            this.Expanded = !this.Expanded;
            this.ExpanderButtonEnabled = true;
        };

        this.BeginAnimation(ExpanderHeightProperty, doubleAnimation);
    }
}
