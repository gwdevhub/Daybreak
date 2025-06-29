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

    public async Task<ManagedFrame?> GetManagedFrame(string label, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (string.IsNullOrEmpty(label))
        {
            scopedLogger.LogError("Frame label is null or empty");
            return null;
        }

        var frame = await this.gameThreadService.QueueOnGameThread(() => this.uIContextService.GetFrameByLabel(label), cancellationToken);
        if (!frame.IsNull)
        {
            // Frame was already open, we do not want to auto-close it
            scopedLogger.LogInformation("Frame with label {label} is already open", label);
            return new ManagedFrame(frame, () => { });
        }

        var uiAction = GetUIActionFromFrameLabel(label);
        await this.Keypress(uiAction, null, cancellationToken);
        frame = await this.gameThreadService.QueueOnGameThread(() => this.uIContextService.GetFrameByLabel(label), cancellationToken);
        await this.gameThreadService.QueueOnGameThread(() => this.uIContextService.SetFrameVisible(frame, true), cancellationToken);
        return new ManagedFrame(frame, async () =>
        {
            await this.gameThreadService.QueueOnGameThread(async () =>
            {
                if (!this.uIContextService.GetFrameByLabel(label).IsNull)
                {
                    await this.Keypress(uiAction, null, CancellationToken.None);
                }
            }, CancellationToken.None);
        });
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
