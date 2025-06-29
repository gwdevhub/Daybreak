using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Screenshots;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.Windows.Media;

namespace Daybreak.Services.Screenshots;

internal sealed class BackgroundProvider : IBackgroundProvider
{
    private readonly IScreenshotProvider screenshotProvider;
    private readonly IOnlinePictureClient bloogumClient;
    private readonly ILiveOptions<BackgroundProviderOptions> liveOptions;
    private readonly ILogger<BackgroundProvider> logger;

    public BackgroundProvider(
        IScreenshotProvider screenshotProvider,
        IOnlinePictureClient bloogumClient,
        ILiveOptions<BackgroundProviderOptions> liveOptions,
        ILogger<BackgroundProvider> logger)
    {
        this.screenshotProvider = screenshotProvider.ThrowIfNull();
        this.bloogumClient = bloogumClient.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task<BackgroundResponse> GetBackground()
    {
        var creditText = string.Empty;
        ImageSource? maybeImage = default;
        if (this.liveOptions.Value.LocalScreenshotsEnabled)
        {
            maybeImage = await this.screenshotProvider.GetRandomScreenShot().ConfigureAwait(true);
        }

        if ((this.liveOptions.Value.BloogumEnabled &&
            Random.Shared.Next(this.liveOptions.Value.LocalScreenshotsEnabled ? 0 : 50, 101) >= 50) ||
            maybeImage is null)
        {
            (var maybeRemoteImage, var credit) = await new TaskFactory().StartNew(() => this.bloogumClient.GetImage(true), TaskCreationOptions.LongRunning).Unwrap().ConfigureAwait(true);
            maybeImage = maybeRemoteImage;
            creditText = credit;
        }

        return new BackgroundResponse
        {
            ImageSource = maybeImage,
            CreditText = creditText
        };
    }
}
