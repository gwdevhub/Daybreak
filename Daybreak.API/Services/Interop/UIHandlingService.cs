using Daybreak.API.Interop;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using System.Security;

namespace Daybreak.API.Services.Interop;

public sealed class UIHandlingService
    : IHostedService, IHookHealthService
{
    private const string SendUIMessageFile = "FrApi.cpp";
    private const string SendUIMessageAssertion = "msgId >= FRAME_MSG_EX";

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void SendUIMessage(UIMessage uiMessage, nuint wParam, nuint lParam);

    private readonly SemaphoreSlim semaphoreSlim = new(1);
    private readonly MemoryScanningService memoryScanningService;
    private readonly GWHook<SendUIMessage> sendUiMessageHook;
    private readonly ILogger<UIHandlingService> logger;

    private CancellationTokenSource? cts = default;

    public UIHandlingService(
        MemoryScanningService memoryScanningService,
        ILogger<UIHandlingService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.sendUiMessageHook = new GWHook<SendUIMessage>(
            new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAssertion(SendUIMessageFile, SendUIMessageAssertion, 0xcf8, 0))),
            this.OnSendUIMessage);
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
