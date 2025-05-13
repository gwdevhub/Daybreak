using System.Windows.Media;

namespace Daybreak.Models;

public sealed class ColoredTextElement
{
    public SolidColorBrush Color { get; init; } = default!;
    public string Text { get; init; } = default!;
}
