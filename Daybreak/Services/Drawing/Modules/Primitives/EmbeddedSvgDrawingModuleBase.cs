using Svg;
using System;
using System.IO;
using System.Reflection;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class EmbeddedSvgDrawingModuleBase<TDerivingType> : SvgDrawingModuleBase
{
    protected sealed override SvgDocument SvgDocument { get; }

    protected abstract string EmbeddedSvgPath { get; }

    public EmbeddedSvgDrawingModuleBase()
    {
        var maybeResourceStream = Assembly.GetAssembly(typeof(TDerivingType))?.GetManifestResourceStream(this.EmbeddedSvgPath);
        if (maybeResourceStream is not Stream resourceStream)
        {
            throw new InvalidOperationException($"Unable to load embedded svg {this.EmbeddedSvgPath}");
        }

        this.SvgDocument = Svg.SvgDocument.Open<SvgDocument>(resourceStream);
    }
}
