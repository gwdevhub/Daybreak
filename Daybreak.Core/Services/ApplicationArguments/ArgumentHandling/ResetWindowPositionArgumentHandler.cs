using System.Extensions;
using Daybreak.Shared.Services.ApplicationArguments.ArgumentHandling;
using Daybreak.Shared.Services.Screens;
using Microsoft.Extensions.Logging;

namespace Daybreak.Services.ApplicationArguments.ArgumentHandling;

internal sealed class ResetWindowPositionArgumentHandler(
        IScreenManager screenManager,
        ILogger<ResetWindowPositionArgumentHandler> logger)
    : IArgumentHandler
{
    private readonly IScreenManager screenManager = screenManager;
    private readonly ILogger<ResetWindowPositionArgumentHandler> logger = logger;

    public string Identifier => "-reset-window";

    public int ExpectedArgumentCount => 0;

    public void HandleArguments(string[] args)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.HandleArguments), string.Empty);
        if (args.Length != 0)
        {
            scopedLogger.LogError("Unexpected count of arguments received");
            return;
        }

        this.screenManager.ResetSavedPosition();
        this.screenManager.MoveWindowToSavedPosition();
    }
}
