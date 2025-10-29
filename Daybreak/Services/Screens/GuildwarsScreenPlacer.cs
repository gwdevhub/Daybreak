using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.Screens;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;

namespace Daybreak.Services.Screens;

internal sealed class GuildwarsScreenPlacer(
    ILiveUpdateableOptions<GuildWarsScreenPlacerOptions> liveOptions,
    IScreenManager screenManager,
    ILogger<GuildwarsScreenPlacer> logger) : IGuildwarsScreenPlacer
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(5);

    private readonly ILiveUpdateableOptions<GuildWarsScreenPlacerOptions> liveOptions = liveOptions.ThrowIfNull();
    private readonly IScreenManager screenManager = screenManager.ThrowIfNull();
    private readonly ILogger<GuildwarsScreenPlacer> logger = logger.ThrowIfNull();

    public string Name => "Auto screen placer";
    public string Description => "Moves the Guildwars window to the desired screen on launch";
    public bool IsVisible => true;
    public bool IsEnabled
    {
        get => this.liveOptions.Value.Enabled;
        set
        {
            this.liveOptions.Value.Enabled = value;
            this.liveOptions.UpdateOption();
        }
    }
    public bool CanCustomManage => true;
    public bool IsInstalled => true;

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("GuildwarsScreenPlacer does not support manual installation.");
    }

    public Task OnCustomManagement(CancellationToken cancellationToken)
    {
        //TODO: Implement custom management UI
        return Task.CompletedTask;
    }

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken)
    {
        var screen = this.screenManager.Screens.Skip(this.liveOptions.Value.DesiredScreen).FirstOrDefault();
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
