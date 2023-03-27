using Daybreak.Models.Guildwars;
using System.Windows;

namespace Daybreak.Utils;

public static class MathUtils
{
    public static bool PointInsideTrapezoid(Trapezoid trapezoid, Point point)
    {
        /*
         * To determine if a point is inside a trapezoid,
         * perform a raycast and count the number of intersections
         * from the point to the edges of the trapezoid. If the number is odd,
         * the point is inside the trapezoid. Otherwise, it is outside.
         */

        var trapezoidPoints = new Point[]
        {
            new Point(trapezoid.XTL, trapezoid.YT),
            new Point(trapezoid.XTR, trapezoid.YT),
            new Point(trapezoid.XBL, trapezoid.YB),
            new Point(trapezoid.XBR, trapezoid.YB),
        };

        // Check if the point is in any of the triangles of the quad. If true, then the point is in the quad.
        for (var i = 0; i < trapezoidPoints.Length; i++)
        {
            var a = trapezoidPoints[i];
            var b = trapezoidPoints[(i + 1) % trapezoidPoints.Length];
            var c = trapezoidPoints[(i + 2) % trapezoidPoints.Length];
            var v0 = c - a;
            var v1 = b - a;
            var v2 = point - a;

            var d00 = v0 * v0;
            var d01 = v0 * v1;
            var d02 = v0 * v2;
            var d11 = v1 * v1;
            var d12 = v1 * v2;

            var invDenom = 1 / ((d00 * d11) - (d01 * d01));
            var u = ((d11 * d02) - (d01 * d12)) * invDenom;
            var v = ((d00 * d12) - (d01 * d02)) * invDenom;

            if ((u > 0) && (v > 0) && (u + v < 1))
            {
                return true;
            }
        }

        return false;
    }

    public static bool PointInsidePolygon(Point[] polygon, Point point)
    {
        var isInside = false;
        for (var i = 0; i < polygon.Length; i++)
        {
            var j = (i + 1) % polygon.Length;
            var p1 = polygon[i];
            var p2 = polygon[j];
            // Check if the lines cross the horizontal line at Y
            if ((p1.Y <= point.Y && p2.Y > point.Y) || (p2.Y <= point.Y && p1.Y > point.Y))
            {
                // Check the point on the X axis where it crosses the line.
                var cross = (p2.X - p1.X) * (point.Y - p1.Y) / (p2.Y - p1.Y) + p1.X;
                if (cross < point.X)
                {
                    isInside = !isInside;
                }
            }
        }

        return isInside;
    }

    public static bool LinesIntersect(Point p11, Point p12, Point p21, Point p22)
    {
        bool Ccw(Point p1, Point p2, Point p3)
        {
            return (p3.Y - p1.Y) * (p2.X - p1.X) > (p2.Y - p1.Y) * (p3.X - p1.X);
        }

        return Ccw(p11, p21, p22) != Ccw(p12, p21, p22) &&
            Ccw(p11, p12, p21) != Ccw(p11, p12, p22);
    }
}
