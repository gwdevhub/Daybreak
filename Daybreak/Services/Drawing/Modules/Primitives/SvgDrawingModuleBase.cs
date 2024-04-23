using Daybreak.Services.Drawing.Modules.Models;
using Daybreak.Utils;
using Svg;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class SvgDrawingModuleBase : DrawingModuleBase
{
    private const int SvgCacheSize = 100;

    private readonly Dictionary<ColorCombination, WriteableBitmap> bitmapCache = [];

    protected override bool HasMinimumSize => true;

    protected abstract SvgDocument GetSvgDocument(Color fillColor, Color strokeColor);

    protected void DrawSvg(WriteableBitmap bitmap, int x, int y, int entitySize, double angle, Color stroke, Color fill, Color shade)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        var combination = new ColorCombination { StrokeColor = ColorExtensions.AlphaBlend(stroke, shade), FillColor = ColorExtensions.AlphaBlend(fill, shade) };
        if (this.bitmapCache.TryGetValue(combination, out var cachedSvg) is false)
        {
            cachedSvg = this.CreateBitmapCache(combination.FillColor, combination.StrokeColor);
            this.bitmapCache.Add(combination, cachedSvg);
        }

        //Convert to degrees
        angle *= 180 / Math.PI;

        cachedSvg = cachedSvg.RotateFree((int)angle);
        bitmap.Blit(new Rect(x - entitySize, y - entitySize, entitySize + entitySize, entitySize + entitySize), cachedSvg, new Rect(0, 0, cachedSvg.Width, cachedSvg.Height), WriteableBitmapExtensions.BlendMode.Alpha);
    }

    private WriteableBitmap CreateBitmapCache(Color color, Color stroke)
    {
        var bitmap = this.GetSvgDocument(color, stroke).Draw(SvgCacheSize, SvgCacheSize);
        var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        return new WriteableBitmap(bitmapSource);
    }
}
