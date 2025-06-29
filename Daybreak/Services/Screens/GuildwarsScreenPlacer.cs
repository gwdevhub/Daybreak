using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.Screens;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;

namespace Daybreak.Services.Screens;

internal sealed class GuildwarsScreenPlacer : IGuildwarsScreenPlacer
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(5);

    private readonly ILiveUpdateableOptions<LauncherOptions> liveOptions;
    private readonly IScreenManager screenManager;
    private readonly ILogger<GuildwarsScreenPlacer> logger;

    public GuildwarsScreenPlacer(
        ILiveUpdateableOptions<LauncherOptions> liveOptions,
        IScreenManager screenManager,
        ILogger<GuildwarsScreenPlacer> logger)
    {
        this.liveOptions = liveOptions.ThrowIfNull();
        this.screenManager = screenManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public string Name => "Screen placer";
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

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken)
    {
        var screen = this.screenManager.Screens.Skip(this.liveOptions.Value.DesiredGuildwarsScreen).FirstOrDefault();
        if (screen is null)
        {
            return;
        }

        await Task.Delay(Delay, cancellationToken);
        this.screenManager.MoveGuildwarsToScreen(screen);
        return;
    }

    public IEnumerable<string> GetCustomArguments() => [];

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken) => Task.CompletedTask;
}
