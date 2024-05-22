using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Behaviors;
internal sealed class LayoutFreezeDecorator : Decorator
{
    public bool IsLayoutFrozen { get; set; }

    protected override Size MeasureOverride(Size constraint)
    {
        if (this.IsLayoutFrozen)
        {
            return new Size(0, 0);
        }

        return base.MeasureOverride(constraint);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        if (this.IsLayoutFrozen)
        {
            return arrangeSize;
        }

        return base.ArrangeOverride(arrangeSize);
    }
}
