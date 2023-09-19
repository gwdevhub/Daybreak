using Daybreak.Configuration.Options;
using Daybreak.Services.Bloogum;
using Daybreak.Services.Screenshots.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace Daybreak.Services.Screenshots;

public sealed class BackgroundProvider : IBackgroundProvider
{
    private readonly IScreenshotProvider screenshotProvider;
    private readonly IBloogumClient bloogumClient;
    private readonly ILiveOptions<BackgroundProviderOptions> liveOptions;
    private readonly ILogger<BackgroundProvider> logger;

    public BackgroundProvider(
        IScreenshotProvider screenshotProvider,
        IBloogumClient bloogumClient,
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

        if (this.liveOptions.Value.BloogumEnabled &&
            Random.Shared.Next(this.liveOptions.Value.LocalScreenshotsEnabled ? 0 : 50, 101) >= 50)
        {
            maybeImage = await this.bloogumClient.GetImage(true).ConfigureAwait(true);
            creditText = maybeImage is not null ? "http://bloogum.net/guildwars" : string.Empty;
        }

        return new BackgroundResponse
        {
            ImageSource = maybeImage,
            CreditText = creditText
        };
    }
}
