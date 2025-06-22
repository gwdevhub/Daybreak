using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Controls;
public sealed class SpacedStackPanel : StackPanel
{
    public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register(
                nameof(Spacing),
                typeof(double),
                typeof(SpacedStackPanel),
                new FrameworkPropertyMetadata(0d,
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (_, __) => { /* invalidates layout automatically */ }));

    public double Spacing
    {
        get => (double)this.GetValue(SpacingProperty);
        set => this.SetValue(SpacingProperty, value);
    }

    protected override Size MeasureOverride(Size constraint)
    {
        this.ApplySpacing();
        return base.MeasureOverride(constraint);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        this.ApplySpacing();
        return base.ArrangeOverride(arrangeSize);
    }

    protected override void OnVisualChildrenChanged(
        DependencyObject visualAdded, DependencyObject visualRemoved)
    {
        base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        this.ApplySpacing();
    }

    private void ApplySpacing()
    {
        bool firstVisible = true;

        foreach (UIElement child in this.InternalChildren)
        {
            if (child is not FrameworkElement fe)
            {
                continue;
            }

            if (child.Visibility == Visibility.Visible &&
                fe.ActualWidth > 0 &&
                fe.ActualHeight > 0)
            {
                // first visible element ⇒ no leading gap
                fe.Margin = firstVisible
                    ? default
                    : this.Orientation == Orientation.Horizontal
                        ? new Thickness(this.Spacing, 0, 0, 0)   // gap to the left
                        : new Thickness(0, this.Spacing, 0, 0);  // gap on top

                firstVisible = false;
            }
            else
            {
                fe.Margin = default;
            }
        }
    }
}
