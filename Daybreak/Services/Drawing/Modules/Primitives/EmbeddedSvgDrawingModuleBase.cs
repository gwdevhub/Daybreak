using Svg;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class EmbeddedSvgDrawingModuleBase<TDerivingType> : SvgDrawingModuleBase
{
    private const string FillColorPlaceholder = "{FILL}";
    private const string StrokeColorPlaceholder = "{STROKE}";

    protected abstract string EmbeddedSvgPath { get; }

    protected override SvgDocument GetSvgDocument(Color fillColor, Color strokeColor)
    {
        var maybeResourceStream = Assembly.GetAssembly(typeof(TDerivingType))?.GetManifestResourceStream(this.EmbeddedSvgPath);
        if (maybeResourceStream is not Stream resourceStream)
        {
            throw new InvalidOperationException($"Unable to load embedded svg {this.EmbeddedSvgPath}");
        }

        using var textReader = new StreamReader(resourceStream);
        var svg = textReader.ReadToEnd();
        var colorConverter = new ColorConverter();
        var colorHex = "#" + colorConverter.ConvertToInvariantString(fillColor)?[3..];
        var strokeHex = "#" + colorConverter.ConvertToInvariantString(strokeColor)?[3..];
        svg = svg.Replace(FillColorPlaceholder, colorHex)
            .Replace(StrokeColorPlaceholder, strokeHex);

        using var ms = new MemoryStream();
        using var textWriter = new StreamWriter(ms);
        textWriter.Write(svg);
        textWriter.Flush();
        ms.Position = 0;

        return SvgDocument.Open<SvgDocument>(ms);
    }
}
