using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Services.Interop;
using System.Extensions.Core;

namespace Daybreak.API.Services;

public sealed class UIService(
    GameThreadService gameThreadService,
    UIContextService uIContextService,
    ILogger<UIService> logger)
{
    private readonly GameThreadService gameThreadService = gameThreadService;
    private readonly UIContextService uIContextService = uIContextService;
    private readonly ILogger<UIService> logger = logger;

    public async Task<bool> Keypress(UIAction action, WrappedPointer<Frame> frame, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (frame.IsNull)
        {
            scopedLogger.LogError("Frame pointer is null");
            return false;
        }

        var keyDownResult = await this.gameThreadService.QueueOnGameThread(() =>
        {
            if (!this.uIContextService.KeyDown(action, frame))
            {
                scopedLogger.LogError("Failed to send keydown action {action}", action);
                return false;
            }

            return true;
        }, cancellationToken);

        if (!keyDownResult)
        {
            return false;
        }

        var keyUpResult = await this.gameThreadService.QueueOnGameThread(() =>
        {
            if (!this.uIContextService.KeyUp(action, frame))
            {
                scopedLogger.LogError("Failed to send keyup action {action}", action);
                return false;
            }

            return true;
        }, cancellationToken);

        if (!keyUpResult)
        {
            return false;
        }

        return true;
    }

    private static UIAction GetUIActionFromFrameLabel(string frameLabel)
    {
        return frameLabel switch
        {
            "AgentCommander0" => UIAction.OpenHeroCommander1,
            "AgentCommander1" => UIAction.OpenHeroCommander2,
            "AgentCommander2" => UIAction.OpenHeroCommander3,
            "AgentCommander3" => UIAction.OpenHeroCommander4,
            "AgentCommander4" => UIAction.OpenHeroCommander5,
            "AgentCommander5" => UIAction.OpenHeroCommander6,
            "AgentCommander6" => UIAction.OpenHeroCommander7,
            _ => UIAction.None
        };
    }
}
