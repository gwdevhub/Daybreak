using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class StarDrawingModuleBase : DrawingModuleBase
{
    private static readonly Lazy<(Point[] OuterPoints, Point[] InnerPoints)> StarCoordinates = new(GetStarGlyphPoints);

    protected void DrawFilledStar(WriteableBitmap bitmap, int x, int y, int entitySize, Color color)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        (var outerPoints, var innerPoints) = StarCoordinates.Value;
        for (var i = 1; i <= 5; i++)
        {
            var outerPoint = outerPoints[i % 5];
            var innerPointPrev = innerPoints[(i - 1) % 5];
            var innerPointNext = innerPoints[i % 5];
            bitmap.FillTriangle(
                x + (int)(innerPointPrev.X * entitySize), y + (int)(innerPointPrev.Y * entitySize),
                x + (int)(outerPoint.X * entitySize), y + (int)(outerPoint.Y * entitySize),
                x + (int)(innerPointNext.X * entitySize), y + (int)(innerPointNext.Y * entitySize),
                color);
        }

        bitmap.FillTriangle(
            x + (int)(innerPoints[0].X * entitySize), y + (int)(innerPoints[0].Y * entitySize),
            x + (int)(innerPoints[1].X * entitySize), y + (int)(innerPoints[1].Y * entitySize),
            x + (int)(innerPoints[2].X * entitySize), y + (int)(innerPoints[2].Y * entitySize),
            color);

        bitmap.FillTriangle(
            x + (int)(innerPoints[2].X * entitySize), y + (int)(innerPoints[2].Y * entitySize),
            x + (int)(innerPoints[3].X * entitySize), y + (int)(innerPoints[3].Y * entitySize),
            x + (int)(innerPoints[4].X * entitySize), y + (int)(innerPoints[4].Y * entitySize),
            color);

        bitmap.FillTriangle(
            x + (int)(innerPoints[0].X * entitySize), y + (int)(innerPoints[0].Y * entitySize),
            x + (int)(innerPoints[2].X * entitySize), y + (int)(innerPoints[2].Y * entitySize),
            x + (int)(innerPoints[4].X * entitySize), y + (int)(innerPoints[4].Y * entitySize),
            color);
    }

    protected void DrawOutlinedStar(WriteableBitmap bitmap, int x, int y, int entitySize, int thickness, Color color)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        (var outerPoints, var innerPoints) = StarCoordinates.Value;

        for (var i = 1; i <= 5; i++)
        {
            var outerPoint = outerPoints[i % 5];
            var innerPointPrev = innerPoints[(i - 1) % 5];
            var innerPointNext = innerPoints[i % 5];
            bitmap.DrawLineAa(
                x + (int)(innerPointPrev.X * entitySize), y + (int)(innerPointPrev.Y * entitySize),
                x + (int)(outerPoint.X * entitySize), y + (int)(outerPoint.Y * entitySize),
                color,
                thickness);
            bitmap.DrawLineAa(
                x + (int)(outerPoint.X * entitySize), y + (int)(outerPoint.Y * entitySize),
                x + (int)(innerPointNext.X * entitySize), y + (int)(innerPointNext.Y * entitySize),
                color,
                thickness);
        }
    }

    private static (Point[] OuterPoints, Point[] InnerPoints) GetStarGlyphPoints()
    {
        var halfSize = 0.5;

        var outerPoints = new Point[]
        {
            new Point(Math.Cos((2 * Math.PI * 0 / 5) - (Math.PI / 10)), Math.Sin((2 * Math.PI * 0 / 5) - (Math.PI / 10))),
            new Point(Math.Cos((2 * Math.PI * 1 / 5) - (Math.PI / 10)), Math.Sin((2 * Math.PI * 1 / 5) - (Math.PI / 10))),
            new Point(Math.Cos((2 * Math.PI * 2 / 5) - (Math.PI / 10)), Math.Sin((2 * Math.PI * 2 / 5) - (Math.PI / 10))),
            new Point(Math.Cos((2 * Math.PI * 3 / 5) - (Math.PI / 10)), Math.Sin((2 * Math.PI * 3 / 5) - (Math.PI / 10))),
            new Point(Math.Cos((2 * Math.PI * 4 / 5) - (Math.PI / 10)), Math.Sin((2 * Math.PI * 4 / 5) - (Math.PI / 10))),
        };

        var innerPoints = new Point[]
        {
            new Point(halfSize * Math.Cos((2 * Math.PI * 0 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 0 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new Point(halfSize * Math.Cos((2 * Math.PI * 1 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 1 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new Point(halfSize * Math.Cos((2 * Math.PI * 2 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 2 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new Point(halfSize * Math.Cos((2 * Math.PI * 3 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 3 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new Point(halfSize * Math.Cos((2 * Math.PI * 4 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 4 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
        };

        return (outerPoints, innerPoints);
    }
}
