using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using Daybreak.API.Services.Interop;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using static Daybreak.API.Services.Interop.UIContextService;

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

    public async Task<string?> DecodeString(string encoded, Language language, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var taskCompletionSource = new TaskCompletionSource<string?>();
        var byteCount = (encoded.Length + 1) * sizeof(char);
        var encodedPtr = Marshal.AllocHGlobal(byteCount);
        unsafe
        {
            var encodedPtrC = (char*)encodedPtr;
            fixed (char* src = encoded)
            {
                Buffer.MemoryCopy(src, encodedPtrC, byteCount, encoded.Length * sizeof(char));
            }

            encodedPtrC[encoded.Length] = '\0';
        }

        GCHandle callbackHandle = default;
        DecodeStrCallback callback;
        unsafe
        {
            callback = (_, decodePtr) =>
            {
                try
                {
                    if (decodePtr is null)
                    {
                        taskCompletionSource.TrySetResult(null);
                        return;
                    }

                    var decoded = new string(decodePtr);
                    taskCompletionSource.TrySetResult(decoded);
                }
                catch (Exception ex)
                {
                    taskCompletionSource.TrySetException(ex);
                }
            };

            callbackHandle = GCHandle.Alloc(callback);
        }

        try
        {
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

                    var prevLanguage = textParser->Language;
                    if (language is not Language.Unknown)
                    {
                        textParser->Language = language;
                    }

                    this.uIContextService.ValidateDecodeString((char*)encodedPtr, callback, nuint.Zero);
                    textParser->Language = prevLanguage;
                }
            }, cancellationToken);

            return await taskCompletionSource.Task;
        }
        finally
        {
            Marshal.FreeHGlobal(encodedPtr);
            if (callbackHandle.IsAllocated)
            {
                callbackHandle.Free();
            }
        }
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
