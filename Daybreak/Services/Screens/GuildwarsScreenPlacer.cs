using Daybreak.Configuration.Options;
using Daybreak.Services.Scanner;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Screens;

public sealed class GuildwarsScreenPlacer : IGuildwarsScreenPlacer
{
    private const int MaxTries = 10;

    private readonly IGuildwarsMemoryCache guildwarsMemoryCache;
    private readonly ILiveUpdateableOptions<LauncherOptions> liveOptions;
    private readonly IScreenManager screenManager;
    private readonly ILogger<GuildwarsScreenPlacer> logger;

    public GuildwarsScreenPlacer(
        IGuildwarsMemoryCache guildwarsMemoryCache,
        ILiveUpdateableOptions<LauncherOptions> liveOptions,
        IScreenManager screenManager,
        ILogger<GuildwarsScreenPlacer> logger)
    {
        this.guildwarsMemoryCache = guildwarsMemoryCache.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.screenManager = screenManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public bool IsEnabled
    {
        get => this.liveOptions.Value.SetGuildwarsWindowSizeOnLaunch;
        set
        {
            this.liveOptions.Value.SetGuildwarsWindowSizeOnLaunch = value;
            this.liveOptions.UpdateOption();
        }
    }

    public bool IsInstalled => true;

    public async Task OnGuildwarsStarted(Process process)
    {
        var screen = this.screenManager.Screens.Skip(this.liveOptions.Value.DesiredGuildwarsScreen).FirstOrDefault();
        if (screen is null)
        {
            return;
        }

        var tries = 0;
        while (await this.guildwarsMemoryCache.ReadLoginData(CancellationToken.None) is null)
        {
            await Task.Delay(1000);
            tries++;
            if (tries > MaxTries)
            {
                this.logger.LogInformation("Failed to detect startup of Guildwars. Cancelling screen placement operation");
                return;
            }
        }

        this.screenManager.MoveGuildwarsToScreen(screen);
        return;
    }

    public IEnumerable<string> GetCustomArguments() => Enumerable.Empty<string>();

    public Task OnGuildwarsStarting(Process process) => Task.CompletedTask;

    public Task OnGuildWarsCreated(Process process) => Task.CompletedTask;
}
