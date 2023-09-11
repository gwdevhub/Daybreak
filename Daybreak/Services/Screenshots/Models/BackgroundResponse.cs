using System.Windows.Media;

namespace Daybreak.Services.Screenshots.Models;
public sealed class BackgroundResponse
{
    public ImageSource? ImageSource { get; set; }
    public string? CreditText { get; set; }
}
