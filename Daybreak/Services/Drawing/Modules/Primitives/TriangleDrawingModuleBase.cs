using Daybreak.Utils;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class TriangleDrawingModuleBase : DrawingModuleBase
{
    protected void DrawFilledTriangle(WriteableBitmap bitmap, int x, int y, int entitySize, double angle, Color color, Color shade)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        var centerX = x;
        var centerY = y;

        // Original points
        var p1 = new Point(x - entitySize, y - entitySize);
        var p2 = new Point(x + entitySize, y - entitySize);
        var p3 = new Point(x, y + entitySize);

        // Rotated points
        var rp1 = RotatePoint(p1, centerX, centerY, angle);
        var rp2 = RotatePoint(p2, centerX, centerY, angle);
        var rp3 = RotatePoint(p3, centerX, centerY, angle);

        var finalColor = ColorExtensions.AlphaBlend(color, shade);
        bitmap.FillTriangle(
            (int)rp1.X, (int)rp1.Y,
            (int)rp2.X, (int)rp2.Y,
            (int)rp3.X, (int)rp3.Y,
            finalColor);
    }

    protected void DrawOutlinedTriangle(WriteableBitmap bitmap, int x, int y, int entitySize, double angle, int thickness, Color color, Color shade)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        var centerX = x;
        var centerY = y;

        // Original points
        var p1 = new Point(x - entitySize, y - entitySize);
        var p2 = new Point(x + entitySize, y - entitySize);
        var p3 = new Point(x, y + entitySize);

        // Rotated points
        var rp1 = RotatePoint(p1, centerX, centerY, angle);
        var rp2 = RotatePoint(p2, centerX, centerY, angle);
        var rp3 = RotatePoint(p3, centerX, centerY, angle);

        var finalColor = ColorExtensions.AlphaBlend(color, shade);
        bitmap.DrawLineAa(
            (int)rp1.X, (int)rp1.Y,
            (int)rp2.X, (int)rp2.Y,
            finalColor,
            thickness);

        bitmap.DrawLineAa(
            (int)rp2.X, (int)rp2.Y,
            (int)rp3.X, (int)rp3.Y,
            finalColor,
            thickness);

        bitmap.DrawLineAa(
            (int)rp3.X, (int)rp3.Y,
            (int)rp1.X, (int)rp1.Y,
            finalColor,
            thickness);
    }

    private static Point RotatePoint(Point point, int centerX, int centerY, double angleRadians)
    {
        double cosTheta = Math.Cos(angleRadians);
        double sinTheta = Math.Sin(angleRadians);
        double dx = point.X - centerX;
        double dy = point.Y - centerY;

        double newX = centerX + dx * cosTheta - dy * sinTheta;
        double newY = centerY + dx * sinTheta + dy * cosTheta;

        return new Point(newX, newY);
    }
}
