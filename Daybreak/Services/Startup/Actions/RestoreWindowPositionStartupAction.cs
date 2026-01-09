using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Screens;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.Startup.Actions;

//TODO: Implement restoring window position
internal sealed class RestoreWindowPositionStartupAction(
    IScreenManager screenManager) : StartupActionBase
{
    private readonly IScreenManager screenManager = screenManager.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        Task.Factory.StartNew(async () =>
        {
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }
}
