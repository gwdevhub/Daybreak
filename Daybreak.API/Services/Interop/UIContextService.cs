using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Logging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace Daybreak.API.Services.Interop;

public sealed class UIContextService
    : IAddressHealthService, IHostedService, IHookHealthService
{
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
    private delegate void SendUIMessage(UIMessage uiMessage, nuint wParam, nuint lParam);

    private readonly SemaphoreSlim semaphoreSlim = new(1);
    private readonly MemoryScanningService memoryScanningService;
    private readonly GWDelegateCache<CreateHashFromStringFunc> createHashFromString;
    private readonly GWAddressCache frameArrayAddressCache;
    private readonly GWHook<SendUIMessage> sendUiMessageHook;
    private readonly ILogger<UIContextService> logger;

    private CancellationTokenSource? cts = default;

    public UIContextService(
        MemoryScanningService memoryScanningService,
        ILogger<UIContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.createHashFromString = new GWDelegateCache<CreateHashFromStringFunc>(new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAddress(CreateHashFromStringSeq, CreateHashFromStringMask, CreateHashFromStringOffset))));
        this.frameArrayAddressCache = new GWAddressCache(() => this.memoryScanningService.FindAssertion(FrameArrayFile, FrameArrayAssertion, 0, -0x14));
        this.sendUiMessageHook = new GWHook<SendUIMessage>(
            new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAssertion(SendUIMessageFile, SendUIMessageAssertion, 0xcf8, 0))),
            this.OnSendUIMessage);
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

    public List<HookState> GetHookStates()
    {
        return
            [
                new HookState
                {
                    Hooked = this.sendUiMessageHook.Hooked,
                    Name = nameof(this.sendUiMessageHook),
                    TargetAddress = new PointerValue(this.sendUiMessageHook.TargetAddress),
                    ContinueAddress = new PointerValue(this.sendUiMessageHook.ContinueAddress),
                    DetourAddress = new PointerValue(this.sendUiMessageHook.DetourAddress)
                }
            ];
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

        foreach(var frame in *frameArray.Pointer)
        {
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
