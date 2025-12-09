using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.Screenshots;

public interface IScreenshotService
{
    Task<ScreenshotEntry?> GetRandomScreenshot(CancellationToken cancellationToken);
}
