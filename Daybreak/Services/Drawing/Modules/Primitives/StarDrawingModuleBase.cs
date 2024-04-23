using Daybreak.Utils;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class StarDrawingModuleBase : DrawingModuleBase
{
    private static readonly Lazy<(Point[] OuterPoints, Point[] InnerPoints)> StarCoordinates = new(GetStarGlyphPoints);

    protected void DrawFilledStar(WriteableBitmap bitmap, int x, int y, int entitySize, double angle, Color color, Color shade)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        var finalColor = ColorExtensions.AlphaBlend(color, shade);
        (var outerPointsOrig, var innerPointsOrig) = StarCoordinates.Value;
        (var outerPoints, var innerPoints) = RotatePoints(outerPointsOrig, innerPointsOrig, angle);
        for (var i = 1; i <= 5; i++)
        {
            var outerPoint = outerPoints[i % 5];
            var innerPointPrev = innerPoints[(i - 1) % 5];
            var innerPointNext = innerPoints[i % 5];
            bitmap.FillTriangle(
                x + (int)(innerPointPrev.X * entitySize), y + (int)(innerPointPrev.Y * entitySize),
                x + (int)(outerPoint.X * entitySize), y + (int)(outerPoint.Y * entitySize),
                x + (int)(innerPointNext.X * entitySize), y + (int)(innerPointNext.Y * entitySize),
                finalColor);
        }

        bitmap.FillTriangle(
            x + (int)(innerPoints[0].X * entitySize), y + (int)(innerPoints[0].Y * entitySize),
            x + (int)(innerPoints[1].X * entitySize), y + (int)(innerPoints[1].Y * entitySize),
            x + (int)(innerPoints[2].X * entitySize), y + (int)(innerPoints[2].Y * entitySize),
            finalColor);

        bitmap.FillTriangle(
            x + (int)(innerPoints[2].X * entitySize), y + (int)(innerPoints[2].Y * entitySize),
            x + (int)(innerPoints[3].X * entitySize), y + (int)(innerPoints[3].Y * entitySize),
            x + (int)(innerPoints[4].X * entitySize), y + (int)(innerPoints[4].Y * entitySize),
            finalColor);

        bitmap.FillTriangle(
            x + (int)(innerPoints[0].X * entitySize), y + (int)(innerPoints[0].Y * entitySize),
            x + (int)(innerPoints[2].X * entitySize), y + (int)(innerPoints[2].Y * entitySize),
            x + (int)(innerPoints[4].X * entitySize), y + (int)(innerPoints[4].Y * entitySize),
            finalColor);
    }

    protected void DrawOutlinedStar(WriteableBitmap bitmap, int x, int y, int entitySize, double angle, int thickness, Color color, Color shade)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        var finalColor = ColorExtensions.AlphaBlend(color, shade);
        (var outerPointsOrig, var innerPointsOrig) = StarCoordinates.Value;
        (var outerPoints, var innerPoints) = RotatePoints(outerPointsOrig, innerPointsOrig, angle);
        for (var i = 1; i <= 5; i++)
        {
            var outerPoint = outerPoints[i % 5];
            var innerPointPrev = innerPoints[(i - 1) % 5];
            var innerPointNext = innerPoints[i % 5];
            bitmap.DrawLineAa(
                x + (int)(innerPointPrev.X * entitySize), y + (int)(innerPointPrev.Y * entitySize),
                x + (int)(outerPoint.X * entitySize), y + (int)(outerPoint.Y * entitySize),
                finalColor,
                thickness);
            bitmap.DrawLineAa(
                x + (int)(outerPoint.X * entitySize), y + (int)(outerPoint.Y * entitySize),
                x + (int)(innerPointNext.X * entitySize), y + (int)(innerPointNext.Y * entitySize),
                finalColor,
                thickness);
        }
    }

    private static Point RotatePoint(Point originalPoint, double angle)
    {
        return new Point((originalPoint.X * Math.Cos(angle)) - (originalPoint.Y * Math.Sin(angle)),
            (originalPoint.X * Math.Sin(angle)) + (originalPoint.Y * Math.Cos(angle)));
    }

    private static (Point[] OuterPoints, Point[] InnerPoints) RotatePoints(Point[] outerPointsOrig, Point[] innerPointsOrig, double angle)
    {
        var outerPoints = new Point[]
        {
            RotatePoint(outerPointsOrig[0], angle),
            RotatePoint(outerPointsOrig[1], angle),
            RotatePoint(outerPointsOrig[2], angle),
            RotatePoint(outerPointsOrig[3], angle),
            RotatePoint(outerPointsOrig[4], angle),
        };

        var innerPoints = new Point[]
        {
            RotatePoint(innerPointsOrig[0], angle),
            RotatePoint(innerPointsOrig[1], angle),
            RotatePoint(innerPointsOrig[2], angle),
            RotatePoint(innerPointsOrig[3], angle),
            RotatePoint(innerPointsOrig[4], angle),
        };

        return (outerPoints, innerPoints);
    }

    private static (Point[] OuterPoints, Point[] InnerPoints) GetStarGlyphPoints()
    {
        var halfSize = 0.5;

        var outerPoints = new Point[]
        {
            new(Math.Cos((2 * Math.PI * 0 / 5) - (Math.PI / 10)), Math.Sin((2 * Math.PI * 0 / 5) - (Math.PI / 10))),
            new(Math.Cos((2 * Math.PI * 1 / 5) - (Math.PI / 10)), Math.Sin((2 * Math.PI * 1 / 5) - (Math.PI / 10))),
            new(Math.Cos((2 * Math.PI * 2 / 5) - (Math.PI / 10)), Math.Sin((2 * Math.PI * 2 / 5) - (Math.PI / 10))),
            new(Math.Cos((2 * Math.PI * 3 / 5) - (Math.PI / 10)), Math.Sin((2 * Math.PI * 3 / 5) - (Math.PI / 10))),
            new(Math.Cos((2 * Math.PI * 4 / 5) - (Math.PI / 10)), Math.Sin((2 * Math.PI * 4 / 5) - (Math.PI / 10))),
        };

        var innerPoints = new Point[]
        {
            new(halfSize * Math.Cos((2 * Math.PI * 0 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 0 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new(halfSize * Math.Cos((2 * Math.PI * 1 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 1 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new(halfSize * Math.Cos((2 * Math.PI * 2 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 2 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new(halfSize * Math.Cos((2 * Math.PI * 3 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 3 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
            new(halfSize * Math.Cos((2 * Math.PI * 4 / 5) + (2 * Math.PI / 10) - (Math.PI / 10)), halfSize * Math.Sin((2 * Math.PI * 4 / 5) + (2 * Math.PI / 10) - (Math.PI / 10))),
        };

        return (outerPoints, innerPoints);
    }
}
