using System.Windows.Media;
using System.Windows;

namespace Daybreak.Utils;
public static class DependencyObjectExtensions
{
    public static T? FindParent<T>(this DependencyObject child) where T : DependencyObject
    {
        var parentObject = VisualTreeHelper.GetParent(child);
        if (parentObject is null)
        {
            return null;
        }

        var parent = parentObject as T;
        if (parent is not null)
        {
            return parent;
        }
        else
        {
            return FindParent<T>(parentObject);
        }
    }

    public static bool IsElementVisible(this FrameworkElement element, FrameworkElement container)
    {
        if (!element.IsVisible)
            return false;

        var bounds = element.TransformToAncestor(container)
                             .TransformBounds(new Rect(0.0, 0.0, element.RenderSize.Width, element.RenderSize.Height));
        var viewport = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);

        return viewport.IntersectsWith(bounds);
    }
}
