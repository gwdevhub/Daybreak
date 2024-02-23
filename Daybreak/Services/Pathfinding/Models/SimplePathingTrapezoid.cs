using Daybreak.Models.Guildwars;
using Daybreak.Utils;
using System;
using System.Numerics;

namespace Daybreak.Services.Pathfinding.Models;


internal sealed class SimplePathingTrapezoid
{
    public enum AdjacentSide
    {
        None,
        BottomTop,
        TopBottom,
        LeftRight,
        RightLeft
    }

    public uint Id { get; }
    public uint Layer { get; }
    public Vector2 A { get; }
    public Vector2 B { get; }
    public Vector2 C { get; }
    public Vector2 D { get; }

    public SimplePathingTrapezoid(Trapezoid trapezoid, uint layer)
    {
        this.Id = (uint)trapezoid.Id;
        this.A = new Vector2(trapezoid.XTL, trapezoid.YT);
        this.B = new Vector2(trapezoid.XBL, trapezoid.YB);
        this.C = new Vector2(trapezoid.XBR, trapezoid.YB);
        this.D = new Vector2(trapezoid.XTR, trapezoid.YT);
        this.Layer = layer;
    }

    public AdjacentSide Touching(SimplePathingTrapezoid rhs)
    {
        if (this.A.X != this.D.X && rhs.B.X != rhs.C.X && this.A.Y == this.B.Y)
        {
            //a bot, b top
            if (MathUtils.Collinear(this.A, this.D, rhs.B, rhs.C))
            {
                return AdjacentSide.BottomTop;
            }
        }

        if (this.B.X != this.C.X && rhs.A.X != rhs.D.X && this.B.Y == rhs.A.Y)
        {
            //a top, b bot
            if (MathUtils.Collinear(this.C, this.B, rhs.D, rhs.A))
            {
                return AdjacentSide.TopBottom;
            }
        }

        //a right, b left
        if (MathUtils.Collinear(this.A, this.B, rhs.C, rhs.D))
        {
            return AdjacentSide.RightLeft;
        }

        //a left, b right
        if (MathUtils.Collinear(this.D, this.C, rhs.B, rhs.A))
        {
            return AdjacentSide.LeftRight;
        }

        return AdjacentSide.None;
    }

    public AdjacentSide TouchingHeight(SimplePathingTrapezoid rhs)
    {
        if (this.A.X != this.D.X && rhs.B.X != rhs.C.X && this.A.Y == rhs.B.Y)
        {
            //a bot, b top
            if (MathUtils.Collinear(this.A, this.D, rhs.B, rhs.C))
            {
                var dh = (Math.Abs(Height(this.A, this.Layer) - Height(rhs.B, rhs.Layer)) + Math.Abs(Height(this.D, this.Layer) - Height(rhs.C, rhs.Layer))) / 2.0f;
                if (dh > 1)
                {
                    return AdjacentSide.None;
                }

                return AdjacentSide.BottomTop;
            }
        }
        if (this.B.X != this.C.X && rhs.A.X != rhs.D.X && this.B.Y == rhs.A.Y)
        {
            //a top, b bot
            if (MathUtils.Collinear(this.C, this.B, rhs.D, rhs.A))
            {
                float dh = (Math.Abs(Height(this.B, this.Layer) - Height(rhs.A, rhs.Layer)) + Math.Abs(Height(this.C, this.Layer) - Height(rhs.D, rhs.Layer))) / 2.0f;
                if (dh > 1)
                {
                    return AdjacentSide.None;
                }

                return AdjacentSide.TopBottom;
            }
        }

        //a right, b left
        if (MathUtils.Collinear(this.A, this.B, rhs.C, rhs.D))
        {
            var dh = (Math.Abs(Height(this.A, this.Layer) - Height(rhs.D, rhs.Layer)) + Math.Abs(Height(this.B, this.Layer) - Height(rhs.C, rhs.Layer))) / 2.0f;
            if (dh > 1)
            {
                return AdjacentSide.None;
            }

            return AdjacentSide.RightLeft;
        }
        //a left, b right
        if (MathUtils.Collinear(this.D, this.C, rhs.B, rhs.A))
        {
            var dh = (Math.Abs(Height(this.C, this.Layer) - Height(rhs.B, rhs.Layer)) + Math.Abs(Height(this.D, this.Layer) - Height(rhs.A, rhs.Layer))) / 2.0f;
            if (dh > 1)
            {
                return AdjacentSide.None;

            }

            return AdjacentSide.LeftRight;
        }

        return AdjacentSide.None;
    }

    internal static float Height(Vector2 p, uint layer){
        return layer;
    }
}
