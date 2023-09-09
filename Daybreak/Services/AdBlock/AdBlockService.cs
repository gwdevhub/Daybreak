using Daybreak.Services.BrowserExtensions;
using Daybreak.Services.Downloads;
using Microsoft.Extensions.Logging;

namespace Daybreak.Services.AdBlock;
public sealed class AdBlockService : ChromeExtensionBase<AdBlockService>
{
    public override string ExtensionId { get; } = "cfhdojbkjhnklbpkdaibdccddilifddb";
    public override string ExtensionName { get; } = "AdBlockPlus";

    public AdBlockService(
        IDownloadService downloadService,
        ILogger<AdBlockService> logger)
        : base(downloadService, logger)
    {
    }
}
