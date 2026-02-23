using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using Daybreak.API.Services.Interop;
using System.Extensions.Core;

namespace Daybreak.API.Services;

public sealed class UIService(
    GameThreadService gameThreadService,
    GameContextService gameContextService,
    UIContextService uIContextService,
    ILogger<UIService> logger)
{
    private readonly GameThreadService gameThreadService = gameThreadService;
    private readonly GameContextService gameContextService = gameContextService;
    private readonly UIContextService uIContextService = uIContextService;
    private readonly ILogger<UIService> logger = logger;

    public async Task<bool> Keypress(ControlAction action, WrappedPointer<Frame> frame, CancellationToken cancellationToken)
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

    public async Task<string?> DecodeString(string encoded, Language language, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var prevLanguage = await this.gameThreadService.QueueOnGameThread<Language?>(() =>
        {
            unsafe
            {
                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull)
                {
                    scopedLogger.LogError("Game context is null");
                    return null;
                }

                var textParser = gameContext.Pointer->TextParserContext;
                if (textParser is null)
                {
                    scopedLogger.LogError("Text parser context is null");
                    return null;
                }

                var prevLanguage = textParser->Language;
                if (language is not Language.Unknown)
                {
                    textParser->Language = language;
                }

                return prevLanguage;
            }
        }, cancellationToken);

        var decoded = await this.uIContextService.AsyncDecodeStringAsync(encoded, cancellationToken);
        await this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull)
                {
                    scopedLogger.LogError("Game context is null");
                    return;
                }

                var textParser = gameContext.Pointer->TextParserContext;
                if (textParser is null)
                {
                    scopedLogger.LogError("Text parser context is null");
                    return;
                }

                if (prevLanguage.HasValue)
                {
                    textParser->Language = prevLanguage.Value;
                }
            }
        }, cancellationToken);

        return decoded;
    }

    private static ControlAction GetUIActionFromFrameLabel(string frameLabel)
    {
        return frameLabel switch
        {
            "AgentCommander0" => ControlAction.OpenHeroCommander1,
            "AgentCommander1" => ControlAction.OpenHeroCommander2,
            "AgentCommander2" => ControlAction.OpenHeroCommander3,
            "AgentCommander3" => ControlAction.OpenHeroCommander4,
            "AgentCommander4" => ControlAction.OpenHeroCommander5,
            "AgentCommander5" => ControlAction.OpenHeroCommander6,
            "AgentCommander6" => ControlAction.OpenHeroCommander7,
            _ => ControlAction.None
        };
    }
}
