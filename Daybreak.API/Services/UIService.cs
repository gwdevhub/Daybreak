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

    public async Task<bool> Keypress(ControlAction action, WrappedPointer<Frame> frame, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var keyDownResult = await this.gameThreadService.QueueOnGameThread(() =>
        {
            if (!this.uIContextService.KeyDown((GWCA.GW.UI.ControlAction)action, frame))
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
            if (!this.uIContextService.KeyUp((GWCA.GW.UI.ControlAction)action, frame))
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

    public async Task<string?> DecodeString(string encoded, GWCA.GW.Constants.Language language, CancellationToken cancellationToken)
    {
        // GWCA's AsyncDecodeStr handles language switching internally (sets language, decodes, restores)
        return await this.uIContextService.AsyncDecodeStringAsync(encoded, language, cancellationToken);
    }

    private static ControlAction GetUIActionFromFrameLabel(string frameLabel)
    {
        return frameLabel switch
        {
            "AgentCommander0" => ControlAction.ControlAction_OpenHeroCommander1,
            "AgentCommander1" => ControlAction.ControlAction_OpenHeroCommander2,
            "AgentCommander2" => ControlAction.ControlAction_OpenHeroCommander3,
            "AgentCommander3" => ControlAction.ControlAction_OpenHeroCommander4,
            "AgentCommander4" => ControlAction.ControlAction_OpenHeroCommander5,
            "AgentCommander5" => ControlAction.ControlAction_OpenHeroCommander6,
            "AgentCommander6" => ControlAction.ControlAction_OpenHeroCommander7,
            _ => ControlAction.ControlAction_None
        };
    }
}
