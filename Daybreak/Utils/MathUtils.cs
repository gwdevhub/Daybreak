using System.Windows;

namespace Daybreak.Utils;

public static class MathUtils
{
    public static Point ClosestPointOnLineSegment(Point v1, Point v2, Point p)
    {
        var d = v2 - v1;
        var distance = d.LengthSquared;
        var nx = (p - v1) * d / distance;

        if (nx < 0)
        {
            return v1;
        }
        else if (nx > 1)
        {
            return v2;
        }
        else
        {
            return (d * nx) + v1;
        }
    }
}
