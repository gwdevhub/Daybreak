using Svg;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class SvgDrawingModuleBase : DrawingModuleBase
{
    private const int SvgCacheSize = 100;

    private readonly Lazy<WriteableBitmap> bitmapCache;

    protected abstract SvgDocument SvgDocument { get; }

    protected override bool HasMinimumSize => true;

    public SvgDrawingModuleBase()
    {
        this.bitmapCache = new Lazy<WriteableBitmap>(this.CreateBitmapCache, true);
    }

    protected void DrawSvg(WriteableBitmap bitmap, int x, int y, int entitySize)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        var cachedSvg = this.bitmapCache.Value;
        bitmap.Blit(new Rect(x - entitySize, y - entitySize, entitySize + entitySize, entitySize + entitySize), cachedSvg, new Rect(0, 0, cachedSvg.Width, cachedSvg.Height), WriteableBitmapExtensions.BlendMode.Alpha);
    }

    private WriteableBitmap CreateBitmapCache()
    {
        var bitmap = this.SvgDocument.Draw(SvgCacheSize, SvgCacheSize);
        var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        return new WriteableBitmap(bitmapSource);
    }
}
