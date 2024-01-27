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
}
