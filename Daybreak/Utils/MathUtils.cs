﻿using Daybreak.Models.Guildwars;
using System;
using System.Windows;

namespace Daybreak.Utils;

public static class MathUtils
{
    public static bool PointInsideTrapezoid(Trapezoid trapezoid, Point point)
    {
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

    public static bool PointInsideTriangle(Point t1, Point t2, Point t3, Point p)
    {
        var d1 = Sign(p, t1, t2);
        var d2 = Sign(p, t2, t3);
        var d3 = Sign(p, t3, t1);

        var has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        var has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(has_neg && has_pos);
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

    public static bool LineSegmentsIntersect(Point p1, Point p2, Point p3, Point p4, out Point? intersectionPoint, double epsilon = 0)
    {
        intersectionPoint = default;
        var denominator = ((p1.X - p2.X) * (p3.Y - p4.Y)) - ((p1.Y - p2.Y) * (p3.X - p4.X));

        // If denominator is 0, lines are either parallel or colinear
        if (denominator == 0)
        {
            // Lines are vertical. Slope is undefined
            if (p2.X - p1.X == 0)
            {
                // Lines are not collinear if they're not on the same X-coordinate
                if (p3.X - p1.X != 0)
                {
                    return false;
                }

                // If p3 is between p1 and p2, the lines are overlapping.
                if (p3.Y > Math.Min(p2.Y, p1.Y) &&
                    p3.Y < Math.Max(p2.Y, p1.Y))
                {
                    intersectionPoint = p3;
                    return true;
                }

                // If p4 is between p1 and p2, the lines are overlapping.
                if (p4.Y > Math.Min(p2.Y, p1.Y) &&
                    p4.Y < Math.Max(p2.Y, p1.Y))
                {
                    intersectionPoint = p3;
                    return true;
                }

                return false;
            }

            // Calculate the slope
            var slope = (p2.Y - p1.Y) / (p2.X - p1.X);

            /*
             * The lines are colinear if the equation for line1 is equal with the equation for line2.
             * This means that we should be able to substitute p1 with p3 or p2 with p4.
             * We will substitute p1 with p3.
             */
            if (p2.Y - p3.Y != slope * (p2.X - p3.X))
            {
                return false;
            }

            // If p3 is between p1 and p2, the lines are overlapping.
            if (p3.Y > Math.Min(p2.Y, p1.Y) &&
                p3.Y < Math.Max(p2.Y, p1.Y))
            {
                intersectionPoint = p3;
                return true;
            }

            // If p4 is between p1 and p2, the lines are overlapping.
            if (p4.Y > Math.Min(p2.Y, p1.Y) &&
                p4.Y < Math.Max(p2.Y, p1.Y))
            {
                intersectionPoint = p3;
                return true;
            }

            return false;
        }

        var x = ((((p1.X * p2.Y) - (p1.Y * p2.X)) * (p3.X - p4.X)) - ((p1.X - p2.X) * ((p3.X * p4.Y) - (p3.Y * p4.X)))) / denominator;
        var y = ((((p1.X * p2.Y) - (p1.Y * p2.X)) * (p3.Y - p4.Y)) - ((p1.Y - p2.Y) * ((p3.X * p4.Y) - (p3.Y * p4.X)))) / denominator;
        intersectionPoint = new Point(x, y);
        return
            x >= Math.Min(p1.X, p2.X) - epsilon &&
            x <= Math.Max(p1.X, p2.X) + epsilon &&
            x >= Math.Min(p3.X, p4.X) - epsilon &&
            x <= Math.Max(p3.X, p4.X) + epsilon &&
            y >= Math.Min(p1.Y, p2.Y) - epsilon &&
            y <= Math.Max(p1.Y, p2.Y) + epsilon &&
            y >= Math.Min(p3.Y, p4.Y) - epsilon &&
            y <= Math.Max(p3.Y, p4.Y) + epsilon;
    }

    public static double DistanceBetweenTwoLineSegments(Point line1Start, Point line1End, Point line2Start, Point line2End)
    {
        var closestPoints = new []
        {
            (line2Start, ClosestPointOnLineSegment(line1Start, line1End, line2Start)),
            (line2End, ClosestPointOnLineSegment(line1Start, line1End, line2End)),
            (line1Start, ClosestPointOnLineSegment(line2Start, line2End, line1Start)),
            (line1End, ClosestPointOnLineSegment(line2Start, line2End, line1End))
        };

        var distance = (closestPoints[0].Item1 - closestPoints[0].Item2).LengthSquared;
        for(var i = 1; i < closestPoints.Length; i++)
        {
            var newDistance = (closestPoints[1].Item1 - closestPoints[1].Item2).LengthSquared;
            if (newDistance < distance)
            {
                distance = newDistance;
            }
        }

        return Math.Sqrt(distance);
    }

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

    private static double Sign(Point p1, Point p2, Point p3)
    {
        return ((p1.X - p3.X) * (p2.Y - p3.Y)) - ((p2.X - p3.X) * (p1.Y - p3.Y));
    }
}
