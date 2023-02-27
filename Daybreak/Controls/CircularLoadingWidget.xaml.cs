using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for CircularLoadingWidget.xaml
/// </summary>
public partial class CircularLoadingWidget : UserControl
{
    public CircularLoadingWidget()
    {
        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == IsEnabledProperty)
        {
            if (e.NewValue is true)
            {
                this.ProgressAnimation_BeginStoryboard.Storyboard.Pause();
            }
            else
            {
                this.ProgressAnimation_BeginStoryboard.Storyboard.Resume();
            }
        }
    }
}
