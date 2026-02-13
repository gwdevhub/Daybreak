using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.DirectSong;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Startup.Actions;

internal sealed class UpdateDirectSongAction(
    IDirectSongService directSongService,
    ILogger<UpdateUModAction> logger) : StartupActionBase
{
    private readonly IDirectSongService directSongService = directSongService.ThrowIfNull();
    private readonly ILogger<UpdateUModAction> logger = logger.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var progress = new Progress<ProgressUpdate>();
        if (!this.directSongService.IsInstalled)
        {
            scopedLogger.LogInformation("DirectSong is not installed. Skipping update.");
            return;
        }

        Task.Factory.StartNew(async () =>
        {
            if (!await this.directSongService.IsUpdateAvailable(CancellationToken.None))
            {
                scopedLogger.LogInformation("DirectSong is up to date");
                return;
            }

            await this.directSongService.PerformUpdate(CancellationToken.None);
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }
}
