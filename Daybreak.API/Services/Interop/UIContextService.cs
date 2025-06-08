using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using System.Security;

namespace Daybreak.API.Services.Interop;

public sealed class UIContextService
    : IAddressHealthService, IHostedService, IHookHealthService
{
    private const string GetChildFrameIdFile = "CtlView.cpp";
    private const string GetChildFrameIdAssertion = "pageId";
    private const int GetChildFrameIdOffset = 0x19;
    private const string SetFrameFlagFile = "FrApi.cpp";
    private const string SetFrameFlagAssertion = "frameId";
    private const int SetFrameVisibleOffset = 0x4f9;
    private const int SetFrameDisabledOffset = 0x4e3;
    private static readonly byte[] SendFrameUiMessageByIdSeq = [0x83, 0xFB, 0x46, 0x73, 0x14];
    private const string SendFrameUiMessageByIdMask = "xxxxx";
    private const int SendFrameUiMessageByIdOffset = -0x34;
    private const string SendUIMessageFile = "FrApi.cpp";
    private const string SendUIMessageAssertion = "msgId >= FRAME_MSG_EX";
    private const string CreateHashFromStringMask = "xxxxxxx";
    private const int CreateHashFromStringOffset = 0x7;
    private static readonly byte[] CreateHashFromStringSeq = [0x85, 0xC0, 0x74, 0x0D, 0x6A, 0xFF, 0x50];
    private const string FrameArrayFile = "\\Code\\Engine\\Frame\\FrMsg.cpp";
    private const string FrameArrayAssertion = "frame";

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate uint CreateHashFromStringFunc(char* value, int seed);

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void SendUIMessageFunc(UIMessage uiMessage, nuint wParam, nuint lParam);

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void SetFrameFlagFunc(uint frameId, uint flag);

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint GetChildFrameIdFunc(uint parentFrameId, uint childOffset);

    private readonly SemaphoreSlim semaphoreSlim = new(1);
    private readonly MemoryScanningService memoryScanningService;
    private readonly GWDelegateCache<CreateHashFromStringFunc> createHashFromString;
    private readonly GWDelegateCache<SetFrameFlagFunc> setFrameVisible;
    private readonly GWDelegateCache<SetFrameFlagFunc> setFrameDisabled;
    private readonly GWDelegateCache<GetChildFrameIdFunc> getChildFrameId;
    private readonly GWAddressCache frameArrayAddressCache;
    private readonly GWHook<SendUIMessageFunc> sendUiMessageHook;
    // This fastcall equivalent is <GuildWarsArray<FrameInteractionCallback>*, void*, UIMessage, void*, void*>
    private readonly GWFastCall<nint, nint, UIMessage, nint, nint, GWFastCall.Void> sendFrameUIMessage;
    private readonly ILogger<UIContextService> logger;

    private CancellationTokenSource? cts = default;

    public UIContextService(
        MemoryScanningService memoryScanningService,
        ILogger<UIContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.createHashFromString = new GWDelegateCache<CreateHashFromStringFunc>(new GWAddressCache(() => this.memoryScanningService.FunctionFromNearCall(
                this.memoryScanningService.FindAddress(CreateHashFromStringSeq, CreateHashFromStringMask, CreateHashFromStringOffset))));
        this.frameArrayAddressCache = new GWAddressCache(() => this.memoryScanningService.FindAssertion(FrameArrayFile, FrameArrayAssertion, 0, -0x14));
        this.sendUiMessageHook = new GWHook<SendUIMessageFunc>(
            new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAssertion(SendUIMessageFile, SendUIMessageAssertion, 0xcf8, 0))),
            this.OnSendUIMessage);
        this.sendFrameUIMessage = new(
            new GWAddressCache(() => this.memoryScanningService.FunctionFromNearCall(
                this.memoryScanningService.FindAddress(SendFrameUiMessageByIdSeq, SendFrameUiMessageByIdMask, SendFrameUiMessageByIdOffset) + 0x67)));
        this.setFrameVisible = new GWDelegateCache<SetFrameFlagFunc>(
            new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAssertion(SetFrameFlagFile, SetFrameFlagAssertion, SetFrameVisibleOffset, 0))));
        this.setFrameDisabled = new GWDelegateCache<SetFrameFlagFunc>(
            new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAssertion(SetFrameFlagFile, SetFrameFlagAssertion, SetFrameDisabledOffset, 0))));
        this.getChildFrameId = new GWDelegateCache<GetChildFrameIdFunc>(
            new GWAddressCache(() => this.memoryScanningService.FunctionFromNearCall(
                this.memoryScanningService.FindAssertion(GetChildFrameIdFile, GetChildFrameIdAssertion, 0, GetChildFrameIdOffset))));
    }

    public List<AddressState> GetAddressStates()
    {
        return
        [
            new()
            {
                Name = nameof(this.createHashFromString),
                Address = this.createHashFromString.Cache.GetAddress() ?? 0U
            },
            new()
            {
                Name = nameof(this.frameArrayAddressCache),
                Address = this.frameArrayAddressCache.GetAddress() ?? 0U
            },
            new()
            {
                Name = nameof(this.setFrameVisible),
                Address = this.setFrameVisible.Cache.GetAddress() ?? 0U
            },
            new()
            {
                Name = nameof(this.setFrameDisabled),
                Address = this.setFrameDisabled.Cache.GetAddress() ?? 0U
            },
            new()
            {
                Name = nameof(this.getChildFrameId),
                Address = this.getChildFrameId.Cache.GetAddress() ?? 0U
            },
            new()
            {
                Name = nameof(this.sendFrameUIMessage),
                Address = this.sendFrameUIMessage.Cache.GetAddress() ?? 0U
            }
        ];
    }

    public List<HookState> GetHookStates()
    {
        return
            [
                new HookState
                {
                    Hooked = this.sendUiMessageHook.Hooked,
                    Name = nameof(this.sendUiMessageHook),
                    TargetAddress = this.sendUiMessageHook.TargetAddress,
                    ContinueAddress = this.sendUiMessageHook.ContinueAddress,
                    DetourAddress = this.sendUiMessageHook.DetourAddress
                }
            ];
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.cts?.Dispose();
        this.cts = new CancellationTokenSource();
        return Task.Factory.StartNew(() => this.InitializeHooks(this.cts.Token), this.cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.cts?.Cancel();
        this.cts?.Dispose();
        this.sendUiMessageHook.Dispose();
        return Task.CompletedTask;
    }

    public unsafe WrappedPointer<Frame> GetChildFrame(WrappedPointer<Frame> parent, uint childOffset)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (parent.IsNull)
        {
            scopedLogger.LogError("Parent frame is null");
            return null;
        }

        var getChildFrameId = this.getChildFrameId.GetDelegate();
        if (getChildFrameId is null)
        {
            scopedLogger.LogError("Failed to get GetChildFrameId delegate");
            return null;
        }

        var childFrameId = getChildFrameId(parent.Pointer->FrameId, childOffset);
        if (childFrameId == 0)
        {
            scopedLogger.LogError("No child frame found for parent frame {parent}", parent.Pointer->FrameId);
            return null;
        }

        return this.GetFrameById(childFrameId);
    }

    public WrappedPointer<Frame> GetButtonActionFrame()
    {
        return this.GetChildFrame(this.GetFrameByLabel("Game"), 6);
    }

    public unsafe bool SendFrameUIMessage(WrappedPointer<Frame> frame, UIMessage messageId, void* arg1, void* arg2 = null)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (frame.IsNull)
        {
            scopedLogger.LogError("Frame is null");
            return false;
        }

        this.sendFrameUIMessage.Invoke((nint)(&frame.Pointer->FrameCallbacks), 0, messageId, (nint)arg1, (nint)arg2);
        return true;
    }

    public unsafe bool SetFrameDisabled(WrappedPointer<Frame> frame, bool disabled)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (frame.IsNull)
        {
            scopedLogger.LogError("Frame is null");
            return false;
        }

        var setFrameDisabled = this.setFrameDisabled.GetDelegate();
        if (setFrameDisabled is null)
        {
            scopedLogger.LogError("Failed to get SetFrameDisabled delegate");
            return false;
        }

        setFrameDisabled(frame.Pointer->FrameId, disabled ? 0U : 10U);
        return true;
    }

    public unsafe bool SetFrameVisible(WrappedPointer<Frame> frame, bool visible)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (frame.IsNull)
        {
            scopedLogger.LogError("Frame is null");
            return false;
        }

        var setFrameVisible = this.setFrameVisible.GetDelegate();
        if (setFrameVisible is null)
        {
            scopedLogger.LogError("Failed to get SetFrameVisible delegate");
            return false;
        }

        setFrameVisible(frame.Pointer->FrameId, visible ? 1U : 0U);
        return true;
    }

    public unsafe bool KeyDown(UIAction action, WrappedPointer<Frame> frame)
    {
        if (frame.IsNull)
        {
            frame = this.GetButtonActionFrame();
        }

        this.semaphoreSlim.Wait();
        var packet = new UIPackets.KeyAction((uint)action);
        this.SendFrameUIMessage(frame, UIMessage.KeyDown, &packet);
        this.semaphoreSlim.Release();
        return true;
    }

    public unsafe bool KeyUp(UIAction action, WrappedPointer<Frame> frame)
    {
        if (frame.IsNull)
        {
            frame = this.GetButtonActionFrame();
        }

        this.semaphoreSlim.Wait();
        var packet = new UIPackets.KeyAction((uint)action);
        this.SendFrameUIMessage(frame, UIMessage.KeyUp, &packet);
        this.semaphoreSlim.Release();
        return true;
    }

    public unsafe WrappedPointer<GuildWarsArray<WrappedPointer<Frame>>> GetFrameArray()
    {
        if (this.frameArrayAddressCache.GetAddress() is not nuint address)
        {
            return null;
        }

        return *(GuildWarsArray<WrappedPointer<Frame>>**)address;
    }

    public unsafe WrappedPointer<Frame> GetFrameByLabel(string label)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var frameArray = this.GetFrameArray();
        if (frameArray.IsNull)
        {
            scopedLogger.LogError("Frame array is null");
            return null;
        }

        var hash = this.CreateHashFromString(label, -1);
        if (hash is 0)
        {
            scopedLogger.LogError("Failed to get hash for label {label}", label);
            return null;
        }

        for(var i = 0; i < frameArray.Pointer->Size; i++)
        {
            var frame = frameArray.Pointer->Buffer[i];
            if (frame.Pointer is null ||
                (int)frame.Pointer == -1)
            {
                continue;
            }

            if (frame.Pointer->Relation.FrameHashId == hash)
            {
                return frame;
            }
        }

        scopedLogger.LogWarning("Frame with label {label} not found", label);
        return null;
    }

    public unsafe WrappedPointer<Frame> GetFrameById(uint frameId)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var frameArray = this.GetFrameArray();
        if (frameArray.IsNull ||
            frameArray.Pointer->Size <= frameId)
        {
            scopedLogger.LogError("Frame array is null or frameId {frameId} is out of bounds", frameId);
            return null;
        }

        var frame = frameArray.Pointer->Skip((int)frameId).FirstOrDefault();
        if (frame.IsNull)
        {
            scopedLogger.LogWarning("Frame with id {frameId} not found", frameId);
            return null;
        }

        return frame;
    }

    public unsafe uint CreateHashFromString(string value, int seed)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.createHashFromString.GetDelegate() is not CreateHashFromStringFunc del)
        {
            scopedLogger.LogError("Failed to get CreateHashFromString delegate");
            return 0;
        }

        fixed (char* valuePtr = value)
        {
            return del(valuePtr, seed);
        }
    }

    public void SendMessage(UIMessage message, nuint wParam, nuint lParam)
    {
        this.semaphoreSlim.Wait();
        this.sendUiMessageHook.Continue(message, wParam, lParam);
        this.semaphoreSlim.Release();
    }

    private async ValueTask InitializeHooks(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Initializing hooks");
        while (!this.sendUiMessageHook.EnsureInitialized())
        {
            scopedLogger.LogDebug("Waiting for hook to initialize: {hook}", nameof(this.sendUiMessageHook));
            await Task.Delay(1000, cancellationToken);
        }

        scopedLogger.LogInformation("Hooks initialized");
    }

    private void OnSendUIMessage(UIMessage uiMessage, nuint wParam, nuint lParam)
    {
        this.sendUiMessageHook.Continue(uiMessage, wParam, lParam);
    }
}
