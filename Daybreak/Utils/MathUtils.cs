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

    public static double DistanceBetweenTwoLineSegments(Point line1Start, Point line1End, Point line2Start, Point line2End)
    {
        return DistanceBetweenTwoLineSegments(
            new Vector3((float)line1Start.X, (float)line1Start.Y, 0),
            new Vector3((float)line1End.X, (float)line1End.Y, 0),
            new Vector3((float)line2Start.X, (float)line2Start.Y, 0),
            new Vector3((float)line2End.X, (float)line2End.Y, 0));
    }

    public static float DistanceBetweenTwoLineSegments(Vector3 line1Start, Vector3 line1End, Vector3 line2Start, Vector3 line2End)
    {
        Vector3 u = line1End - line1Start;
        Vector3 v = line2End - line2Start;
        Vector3 w = line1Start - line2Start;

        float a = Vector3.Dot(u, u);
        float b = Vector3.Dot(u, v);
        float c = Vector3.Dot(v, v);
        float d = Vector3.Dot(u, w);
        float e = Vector3.Dot(v, w);

        float D = a * c - b * b;
        float sc, sN, sD = D;
        float tc, tN, tD = D;

        if (D < 0.0001f)
        {
            sN = 0.0f;
            sD = 1.0f;
            tN = e;
            tD = c;
        }
        else
        {
            sN = (b * e - c * d);
            tN = (a * e - b * d);

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
                sN = (-d + b);
                sD = a;
            }
        }

        sc = (Math.Abs(sN) < 0.0001f ? 0.0f : sN / sD);
        tc = (Math.Abs(tN) < 0.0001f ? 0.0f : tN / tD);

        Vector3 dP = w + (sc * u) - (tc * v);

        return dP.Length();
    }
}
