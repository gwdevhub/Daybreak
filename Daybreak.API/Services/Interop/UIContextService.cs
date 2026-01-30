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
    private const int GetChildFrameIdOffset = 0x12;
    private static readonly byte[] GetChildFrameIdPattern = [0xE8];
    private const string GetChildFrameIdMask = "x";

    private static readonly byte[] SendFrameUiMessageSeq = [
        0x83, 0xC1, 0xDC,              // add ecx, -24
        0xE8, 0x00, 0x00, 0x00, 0x00,  // call
        0x8B, 0x4D, 0xFC               // mov ecx, [ebp-04]
    ];
    private const string SendFrameUiMessageMask = "xxxx????xxx";
    private const int SendFrameUiMessageOffset = 0x3;

    private static readonly byte[] SendUIMessageSeq = [0xB9, 0x00, 0x00, 0x00, 0x00, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x5D, 0xC3, 0x89, 0x45, 0x08];
    private const string SendUIMessageMask = "x????x????xxxxx";

    private const string FrameArrayFile = "\\Code\\Engine\\Frame\\FrMsg.cpp";
    private const string FrameArrayAssertion = "frame";

    private static readonly byte[] GetRootFrameSeq = [
        0xA1, 0x00, 0x00, 0x00, 0x00,  // mov eax,[address]
        0x05, 0xD8, 0xFE, 0xFF, 0xFF,  // add eax,FFFFFED8
        0xC3                           // ret
    ];
    private const string GetRootFrameMask = "x????xxxxxx";

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void SendUIMessageFunc(UIMessage uiMessage, nuint wParam, nuint lParam);

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint GetChildFrameIdFunc(uint parentFrameId, uint childOffset);

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate Frame* GetRootFrameFunc();

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate void ValidateAsyncDecodeStr(char* s, DecodeStrCallback callback, void* wParam);

    public unsafe delegate void DecodeStrCallback(void* param, char* s);

    private readonly SemaphoreSlim sendUiMessageSemaphore = new(1);
    private readonly SemaphoreSlim decodeStringSemaphore = new(1);
    private readonly MemoryScanningService memoryScanningService;
    private readonly HashingService hashingService;
    private readonly GWDelegateCache<GetChildFrameIdFunc> getChildFrameId;
    private readonly GWAddressCache frameArrayAddressCache;
    private readonly GWHook<SendUIMessageFunc> sendUiMessageHook;
    private readonly GWDelegateCache<GetRootFrameFunc> getRootFrame;
    private readonly GWDelegateCache<ValidateAsyncDecodeStr> validateAsyncDecodeStr;
    // This fastcall equivalent is <GuildWarsArray<FrameInteractionCallback>*, void*, UIMessage, void*, void*>
    private readonly GWFastCall<nint, nint, UIMessage, nint, nint, GWFastCall.Void> sendFrameUIMessage;
    private readonly ILogger<UIContextService> logger;

    private CancellationTokenSource? cts;

    public UIContextService(
        HashingService hashingService,
        MemoryScanningService memoryScanningService,
        ILogger<UIContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.hashingService = hashingService.ThrowIfNull();
        this.frameArrayAddressCache = new GWAddressCache(() => this.memoryScanningService.FindAssertion(FrameArrayFile, FrameArrayAssertion, 0, -0x14));
        this.sendUiMessageHook = new GWHook<SendUIMessageFunc>(
            new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAddress(SendUIMessageSeq, SendUIMessageMask))),
            this.OnSendUIMessage);
        this.sendFrameUIMessage = new(
            new GWAddressCache(() => this.memoryScanningService.FunctionFromNearCall(
                this.memoryScanningService.FindAddress(SendFrameUiMessageSeq, SendFrameUiMessageMask, SendFrameUiMessageOffset))));
        this.getChildFrameId = new GWDelegateCache<GetChildFrameIdFunc>(
            new GWAddressCache(() =>
            {
                var address = this.memoryScanningService.FindAssertion(GetChildFrameIdFile, GetChildFrameIdAssertion, 0, GetChildFrameIdOffset);
                return this.memoryScanningService.FunctionFromNearCall(
                    this.memoryScanningService.FindAddress(GetChildFrameIdPattern, GetChildFrameIdMask, 0, address, address + 0xFF));
            }));
        this.getRootFrame = new GWDelegateCache<GetRootFrameFunc>(
            new GWAddressCache(() => this.memoryScanningService.FindAddress(GetRootFrameSeq, GetRootFrameMask, 0)));

        this.validateAsyncDecodeStr = new GWDelegateCache<ValidateAsyncDecodeStr>(
            new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
            this.memoryScanningService.FindUseOfString("(codedString[0] & ~WORD_BIT_MORE) >= WORD_VALUE_BASE"))));
    }

    public List<AddressState> GetAddressStates()
    {
        return
        [
            new()
            {
                Name = nameof(this.frameArrayAddressCache),
                Address = this.frameArrayAddressCache.GetAddress() ?? 0U
            },
            new()
            {
                Name = nameof(this.sendFrameUIMessage),
                Address = this.sendFrameUIMessage.Cache.GetAddress() ?? 0U
            },
            new()
            {
                Name = nameof(this.getChildFrameId),
                Address = this.getChildFrameId.Cache.GetAddress() ?? 0U
            },
            new()
            {
                Name = nameof(this.getRootFrame),
                Address = this.getRootFrame.Cache.GetAddress() ?? 0U
            },
            new()
            {
                Name = nameof(this.validateAsyncDecodeStr),
                Address = this.validateAsyncDecodeStr.Cache.GetAddress() ?? 0U
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

    public unsafe WrappedPointer<T> GetFrameContext<T>(WrappedPointer<Frame> frame)
        where T : unmanaged
    {
        if (frame.IsNull || frame.Pointer->FrameCallbacks.Size == 0)
        {
            return null;
        }

        for (uint i = 0; i < frame.Pointer->FrameCallbacks.Size; i++)
        {
            var callback = frame.Pointer->FrameCallbacks.Buffer[i];
            if (callback.UiCtl_Context != null)
            {
                return new WrappedPointer<T>((T*)callback.UiCtl_Context);
            }
        }

        return null;
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

    public bool SetFrameDisabled(WrappedPointer<Frame> frame, bool disabled)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (frame.IsNull)
        {
            scopedLogger.LogError("Frame is null");
            return false;
        }

        SetFrameDisabledInternal(frame, disabled);
        return true;
    }

    public bool SetFrameVisible(WrappedPointer<Frame> frame, bool visible)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (frame.IsNull)
        {
            scopedLogger.LogError("Frame is null");
            return false;
        }

        SetFrameVisibleInternal(frame, visible);
        return true;
    }

    public unsafe bool KeyDown(UIAction action, WrappedPointer<Frame> frame)
    {
        if (frame.IsNull)
        {
            frame = this.GetButtonActionFrame();
        }

        this.sendUiMessageSemaphore.Wait();
        var packet = new UIPackets.KeyAction((uint)action);
        this.SendFrameUIMessage(frame, UIMessage.KeyDown, &packet);
        this.sendUiMessageSemaphore.Release();
        return true;
    }

    public unsafe bool KeyUp(UIAction action, WrappedPointer<Frame> frame)
    {
        if (frame.IsNull)
        {
            frame = this.GetButtonActionFrame();
        }

        this.sendUiMessageSemaphore.Wait();
        var packet = new UIPackets.KeyAction((uint)action);
        this.SendFrameUIMessage(frame, UIMessage.KeyUp, &packet);
        this.sendUiMessageSemaphore.Release();
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

        var hash = this.CreateHashFromString(label);
        if (hash is 0)
        {
            scopedLogger.LogError("Failed to get hash for label {label}", label);
            return null;
        }

        for (var i = 0; i < frameArray.Pointer->Size; i++)
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

    public uint CreateHashFromString(string value)
    {
        return this.hashingService.Hash(value);
    }

    public void SendMessage(UIMessage message, nuint wParam, nuint lParam)
    {
        this.sendUiMessageSemaphore.Wait();
        this.sendUiMessageHook.Continue(message, wParam, lParam);
        this.sendUiMessageSemaphore.Release();
    }

    /// <summary>
    /// Decodes a string asynchronously using the game's internal decoding mechanism.
    /// </summary>
    /// <remarks>
    /// Inside the DecodeStrCallback, the decoded string has to be cleaned up by calling Marshal.FreeHGlobal on the char* pointer.
    /// </remarks>
    public unsafe void ValidateDecodeString(char* encoded, DecodeStrCallback callback, nuint callbackParam)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        this.decodeStringSemaphore.Wait();
        try
        {
            var validateDecodeStrFnc = this.validateAsyncDecodeStr.GetDelegate();
            if (validateDecodeStrFnc is null)
            {
                scopedLogger.LogError("{funcName} is null", nameof(this.validateAsyncDecodeStr));
                return;
            }

            validateDecodeStrFnc(encoded, callback, (void*)callbackParam);
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Encountered exception while decoding string");
        }
        finally
        {
            this.decodeStringSemaphore.Release();
        }
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

    private static unsafe void SetFrameVisibleInternal(WrappedPointer<Frame> frame, bool visible)
    {
        uint* pState = &frame.Pointer->FrameState;   // direct field address
        if (visible)
        {
            *pState &= ~0x200u;   // clear "hidden"
        }
        else
        {
            *pState |= 0x200u;   // set "hidden"
        }
    }

    private static unsafe void SetFrameDisabledInternal(WrappedPointer<Frame> frame, bool disabled)
    {
        uint* pState = &frame.Pointer->FrameState;   // same word (offset 0x184)

        if (disabled)
        {
            *pState |= 0x10u;    // set "disabled"
        }
        else
        {
            *pState &= ~0x10u;    // clear
        }
    }
}
