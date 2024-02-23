using Daybreak.Utils;
using System;
using System.Numerics;

namespace Daybreak.Services.Pathfinding.Models;

internal sealed class BoundingBox
{
    public uint BoxId { get; }
    public Vector2 Position { get; }
    public Vector2 Half { get; }
    public SimplePathingTrapezoid Trapezoid;
    
    public BoundingBox(SimplePathingTrapezoid trapezoid)
    {
        this.BoxId = 0;
        this.Trapezoid = trapezoid;
        var minX = Math.Min(trapezoid.B.X, trapezoid.A.X);
        var maxX = Math.Max(trapezoid.C.X, trapezoid.D.X);
        this.Position = new Vector2((minX + maxX) / 2, (trapezoid.A.Y + trapezoid.B.Y) / 2);
        this.Half = new Vector2((maxX - minX) / 2, (trapezoid.A.Y - trapezoid.B.Y) / 2);
    }

    public bool Intersect(Vector2 rhs)
    {
        var d = rhs - this.Position;
        var px = this.Half.X - Math.Abs(d.X);
        if (px <= 0.0f)
        {
            return false;
        }

        var py = this.Half.Y - Math.Abs(d.Y);
        if (py <= 0.0f)
        {
            return false;
        }

        return true;
    }

    public bool Intersect(Vector2 rhs, float radius)
    {
        var d = rhs - this.Position;
        var px = this.Half.X + radius - Math.Abs(d.X);
        if (px <= 0.0f)
        {
            return false;
        }

        var py = this.Half.Y + radius - Math.Abs(d.Y);
        if (py <= 0.0f)
        {
            return false;
        }

        return true;
    }

    public bool Intersect(Vector2 a, Vector2 b, Vector2? padding = default)
    {
        if (!padding.HasValue)
        {
            padding = Vector2.Zero;
        }

        var dist = this.Half + padding.Value;
        if (a.X > this.Position.X + dist.X && b.X > this.Position.X + dist.X)
        {
            return false;
        }

        if (a.X < this.Position.X - dist.X && b.X < this.Position.X - dist.X)
        {
            return false;
        }

        if (a.Y > this.Position.Y + dist.Y && b.Y > this.Position.Y + dist.Y)
        {
            return false;
        }

        if (a.Y < this.Position.Y - dist.Y && b.Y < this.Position.Y - dist.Y)
        {
            return false;
        }

        var delta = b - a;
        var scale = Vector2.One / delta.X;
        var sig = MathUtils.Sign(scale);
        var sq = MathUtils.Hadamard(sig, dist);
        var nearTime = MathUtils.Hadamard(sq, scale);
        var farTime = MathUtils.Hadamard(sq, scale);

        if (nearTime.X > farTime.Y || nearTime.Y > farTime.X)
        {
            return false;
        }

        if (Math.Max(nearTime.X, nearTime.Y) >= 1.0f || Math.Min(farTime.X, farTime.Y) <= 0.0f)
        {
            return false;
        }

        return true;
    }

    public bool Intersect(BoundingBox rhs, Vector2? padding = default)
    {
        if (!padding.HasValue)
        {
            padding = Vector2.Zero;
        }

        var d = rhs.Position - this.Position;
        var px = (rhs.Half.X + this.Half.X + padding.Value.X) - Math.Abs(d.X);
        if (px <= 0.0f)
        {
            return false;
        }

        var py = (rhs.Half.Y + this.Half.Y + padding.Value.Y) - Math.Abs(d.Y);
        if (py <= 0.0f)
        {
            return false;
        }

        return true;
    }
}
