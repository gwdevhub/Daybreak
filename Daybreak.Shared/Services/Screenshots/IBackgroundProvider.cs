using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.Screenshots;

public interface IBackgroundProvider
{
    Task<BackgroundResponse> GetBackground();
}
