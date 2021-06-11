using System.Extensions;
using System.Windows.Extensions.Services;
using System.Windows.Media;

namespace Daybreak.Services.Screenshots
{
    public interface IScreenshotProvider : IApplicationLifetimeService
    {
        Optional<ImageSource> GetRandomScreenShot();
        Optional<ImageSource> GetScreenshot(string name);
    }
}
