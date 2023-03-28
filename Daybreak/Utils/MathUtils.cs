using Daybreak.Models.Guildwars;
using System.Numerics;
using System;
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

    public static bool LineSegmentsIntersect(Point p1, Point p2, Point p3, Point p4)
    {
        if (DoColinearLineSegmentsIntersect(p1, p2, p3, p4))
        {
            return true;
        }

        var dx12 = p2.X - p1.X;
        var dy12 = p2.Y - p1.Y;
        var dx34 = p4.X - p3.X;
        var dy34 = p4.Y - p3.Y;

        var denominator = (dy12 * dx34) - (dx12 * dy34);

        if (denominator == 0)
            return false;

        var t1 = (((p1.X - p3.X) * dy34) + ((p3.Y - p1.Y) * dx34)) / denominator;

        if (double.IsInfinity(t1))
            return false;

        var intersectionPoint = new Point(p1.X + (dx12 * t1), p1.Y + (dy12 * t1));

        if (p1.X == p2.X)
            return intersectionPoint.Y >= Math.Min(p3.Y, p4.Y) && intersectionPoint.Y <= Math.Max(p3.Y, p4.Y);
        else
            return intersectionPoint.X >= Math.Min(p1.X, p2.X) && intersectionPoint.X <= Math.Max(p1.X, p2.X);
    }

    public static double DistanceBetweenTwoLineSegments(Point line1Start, Point line1End, Point line2Start, Point line2End)
    {
        var u = line1End - line1Start;
        var v = line2End - line2Start;
        var w = line1Start - line2Start;

        var a = u * u;
        var b = u * v;
        var c = v * v;
        var d = u * v;
        var e = v * w;

        var D = (a * c) - (b * b);
        var sD = D;
        var tD = D;

        double sN;
        double tN;
        if (D < 0.0001f)
        {
            sN = 0.0f;
            sD = 1.0f;
            tN = e;
            tD = c;
        }
        else
        {
            sN = (b * e) - (c * d);
            tN = (a * e) - (b * d);

            if (sN < 0.0f)
            {
                sN = 0.0f;
                tN = e;
                tD = c;
            }
            else if (sN > sD)
            {
                sN = sD;
                tN = e + b;
                tD = c;
            }
        }

        if (tN < 0.0f)
        {
            tN = 0.0f;

            if (-d < 0.0f)
                sN = 0.0f;
            else if (-d > a)
                sN = sD;
            else
            {
                sN = -d;
                sD = a;
            }
        }
        else if (tN > tD)
        {
            tN = tD;

            if ((-d + b) < 0.0f)
                sN = 0.0f;
            else if ((-d + b) > a)
                sN = sD;
            else
            {
                sN = -d + b;
                sD = a;
            }
        }

        double sc = Math.Abs(sN) < 0.0001f ? 0.0f : sN / sD;
        double tc = Math.Abs(tN) < 0.0001f ? 0.0f : tN / tD;

        var dP = w + (sc * u) - (tc * v);

        return dP.Length;
    }

    public static Point ClosestPointOnLineSegment(Point v1, Point v2, Point p)
    {
        var length = (v2 - v1).Length;
        var dir = v2 - v1;
        dir.Normalize();
        var dot = dir * (p - v1);
        dot = Math.Clamp(dot, 0, length);
        return v1 + (dir * dot);
    }

    public static bool DoColinearLineSegmentsIntersect(Point p1, Point p2, Point p3, Point p4)
    {
        if (p1.X > p2.X)
        {
            Swap(ref p1, ref p2);
        }

        if (p3.X > p4.X)
        {
            Swap(ref p3, ref p4);
        }

        if (p1.X > p4.X || p3.X > p2.X)
        {
            return false;
        }

        if (p1.Y > p2.Y)
        {
            Swap(ref p1, ref p2);
        }

        if (p3.Y > p4.Y)
        {
            Swap(ref p3, ref p4);
        }

        if (p1.Y > p4.Y || p3.Y > p2.Y)
        {
            return false;
        }

        return true;
    }

    private static void Swap(ref Point point1, ref Point point2)
    {
        var temp = point1;
        point1 = point2;
        point2 = temp;
    }
}
