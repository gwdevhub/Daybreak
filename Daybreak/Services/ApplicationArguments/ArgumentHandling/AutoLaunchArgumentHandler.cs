using Daybreak.Shared.Services.ApplicationArguments.ArgumentHandling;
using Daybreak.Shared.Services.LaunchConfigurations;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.ApplicationArguments.ArgumentHandling;
internal sealed class AutoLaunchArgumentHandler(
    //IViewManager viewManager,
    ILaunchConfigurationService launchConfigurationService,
    ILogger<AutoLaunchArgumentHandler> logger) : IArgumentHandler
{
    private static readonly TimeSpan StartupDelay = TimeSpan.FromSeconds(1);

    //private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly ILaunchConfigurationService launchConfigurationService = launchConfigurationService.ThrowIfNull();
    private readonly ILogger<AutoLaunchArgumentHandler> logger = logger.ThrowIfNull();

    public string Identifier => "-auto-launch";
    public int ExpectedArgumentCount => 1;

    public async void HandleArguments(string[] args)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.HandleArguments), string.Empty);
        if (args.Length != 1)
        {
            scopedLogger.LogError("Unexpected count of arguments received");
            return;
        }

        var desiredConfig = this.launchConfigurationService.GetLaunchConfigurations().FirstOrDefault(l => l.Identifier?.Replace("-", "") == args[0].Replace("-", ""));
        if (desiredConfig is null)
        {
            scopedLogger.LogError($"Could not find configuration with id {args[0]}");
            return;
        }

        await Task.Delay(StartupDelay);
        //this.viewManager.ShowView<LauncherView>(desiredConfig);
    }
}
