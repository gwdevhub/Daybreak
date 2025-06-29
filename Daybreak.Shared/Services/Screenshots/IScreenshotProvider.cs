using System.Windows.Media;

namespace Daybreak.Shared.Services.Screenshots;

public interface IScreenshotProvider
{
    Task<ImageSource?> GetRandomScreenShot();
}
