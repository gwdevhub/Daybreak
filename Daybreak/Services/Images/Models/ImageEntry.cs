using System.Windows.Media;

namespace Daybreak.Services.Images.Models;

internal sealed class ImageEntry
{
    public string Uri { get; init; } = default!;
    public ImageSource ImageSource { get; init; } = default!;
    public double Size { get; init; }
}
