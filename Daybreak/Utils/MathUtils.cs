using Daybreak.Models.Guildwars;
using System;
using System.Numerics;
using System.Windows;

namespace Daybreak.Utils;

public static class MathUtils
{
    public static Vector2 Intercept(Vector2 chaser, float chaserVelocity, Vector2 target, Vector2 direction, float targetValocity, float radius)
    {
        var vc = chaserVelocity;
        var v = direction * targetValocity;
        var vt = targetValocity;
        var x = target - chaser;
        var r = radius;

        if (vc == 0.0f) {
            return chaser;
        }

        var a = (vc * vc) - (vt * vt); //vt*vt == v*v
        var b = -2.0f * (Vector2.Dot(x, v) + (vt * r));
        var c = -Vector2.Dot(x, x) - (2.0f * r * Vector2.Dot(x, direction)) - (r * r); // (r * r * Dot(v, v) / (vt * vt));

        float t;
        if (Math.Abs(a) < 0.001f)
        {
            if (Math.Abs(b) < 0.0001f)
            {
                return target + (r * direction);
            }

            t = -c / b;
            t = Math.Max(t, 0.0f);
        }
        else
        {
            float s = (b * b) - (4.0f * a * c);
            if (s < 0.0f)
            {
                s = 0.0f; // return target + r * direction;
            }

            s = MathF.Sqrt(s);
            a = 1.0f / (2.0f * a);

            float t1 = (-b + s) * a;
            float t2 = (-b - s) * a;
            if (t1 > 0.0f && t2 > 0.0f)
            {
                t = Math.Min(t1, t2);
            }
            else
            {
                t = Math.Min(t1, t2);
                t = Math.Min(t, 0.0f);
            }
        }

        var p = x + (v * t) + (r * direction);
        return chaser + p;
    }

    public static Vector2 GetVector2(this Vector3 vector3)
    {
        return new Vector2(vector3.X, vector3.Y);
    }

    public static Vector2 Hadamard(Vector2 lhs, Vector2 rhs)
    {
        return new Vector2(lhs.X * rhs.X, lhs.Y * rhs.Y);
    }

    public static Vector2 Sign(Vector2 a)
    {
        return new Vector2(Math.Sign(a.X), Math.Sign(a.Y));
    }

    public static float Cross(Vector2 lhs, Vector2 rhs)
    {
        return (lhs.X * rhs.Y) - (lhs.Y * rhs.X);
    }

    public static bool OnSegment(Vector2 p, Vector2 q, Vector2 r)
    {
        if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
            q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
        {
            return true;
        }

        return false;
    }

    public static bool Intersect(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2)
    {
        var eps = 0.001f;
        var denom = ((q2.Y - p2.Y) * (q1.X - p1.X)) - ((q2.X - p2.X) * (q1.Y - p1.Y));

        /* Are the line parallel */
        if (Math.Abs(denom) < eps)
        {
            return false;
        }

        var numera = ((q2.X - p2.X) * (p1.Y - p2.Y)) - ((q2.Y - p2.Y) * (p1.X - p2.X));
        var numerb = ((q1.X - p1.X) * (p1.Y - p2.Y)) - ((q1.Y - p1.Y) * (p1.X - p2.X));

        /* Are the line coincident? */
        if (Math.Abs(numera) < eps && Math.Abs(numerb) < eps)
        {
            return true;
        }

        /* Is the intersection along the the segments */
        var mua = numera / denom;
        var mub = numerb / denom;
        if (mua < 0.0f || mua > 1.0f || mub < 0.0f || mub > 1.0f)
        {
            return false;
        }

        return true;
    }

    public static bool Between(float x, float min, float max)
    {
        if (min <= max)
        {
            return min <= x && x <= max;
        }
        else
        {
            return min >= x && x >= max;
        }   
    }

    public static bool Collinear(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        var tolerance = 0.1f;

        var s = b2 - b1;
        var inv = 1.0f / Vector2.Dot(s, s) * s;
        var q1 = b1 + (Vector2.Dot(a1 - b1, s) * inv);
        var q2 = b1 + (Vector2.Dot(a2 - b1, s) * inv);

        if (GetSquaredNorm(a1 - q1) > tolerance || GetSquaredNorm(a2 - q2) > tolerance)
            return false;

        return OnSegment(a1, b1, a2) || OnSegment(a1, b2, a2) || OnSegment(b1, a1, b2) || OnSegment(b1, a2, b2);
    }

    public static float GetSquaredNorm(Vector2 p)
    {
        return (p.X * p.X) + (p.Y * p.Y);
    }

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

    public static Point[] GetTrapezoidPoints(Trapezoid trapezoid)
    {
        return new Point[]
        {
            new Point(trapezoid.XTL, trapezoid.YT),
            new Point(trapezoid.XTR, trapezoid.YT),
            new Point(trapezoid.XBR, trapezoid.YB),
            new Point(trapezoid.XBL, trapezoid.YB)
        };
    }

    public static uint RoundUpToTheNextHighestPowerOf2(uint n)
    {
        n--;
        n |= n >> 1;
        n |= n >> 2;
        n |= n >> 4;
        n |= n >> 8;
        n |= n >> 16;
        n++;
        return n;
    }
}
