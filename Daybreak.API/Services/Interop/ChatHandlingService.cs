using Daybreak.API.Interop;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using System.Security;

namespace Daybreak.API.Services.Interop;

public sealed class ChatHandlingService
    : IHostedService, IHookHealthService
{
    private const string AddToChatLogMask = "xxxxxx";
    private static readonly byte[] AddToChatLogSeq = [0x40, 0x25, 0xFF, 0x01, 0x00, 0x00];

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool AddToChatLog([MarshalAs(UnmanagedType.LPWStr)] string message, Channel channel);

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWHook<AddToChatLog> addToChatLogHook;
    private readonly ILogger<ChatHandlingService> logger;

    private CancellationTokenSource? cts = default;

    public ChatHandlingService(
        MemoryScanningService memoryScanningService,
        ILogger<ChatHandlingService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.addToChatLogHook = new GWHook<AddToChatLog>(
            new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAddress(AddToChatLogSeq, AddToChatLogMask))),
            this.OnAddToChatLog);
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
        this.addToChatLogHook.Dispose();
        return Task.CompletedTask;
    }

    public List<HookState> GetHookStates()
    {
        return
            [
                new HookState
                {
                    Hooked = this.addToChatLogHook.Hooked,
                    Name = nameof(this.addToChatLogHook),
                    TargetAddress = new PointerValue(this.addToChatLogHook.TargetAddress),
                    ContinueAddress = new PointerValue(this.addToChatLogHook.ContinueAddress),
                    DetourAddress = new PointerValue(this.addToChatLogHook.DetourAddress)
                }
            ];
    }

    private async ValueTask InitializeHooks(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Initializing hooks");
        while (!this.addToChatLogHook.EnsureInitialized())
        {
            scopedLogger.LogDebug("Waiting for hook to initialize: {hook}", nameof(this.addToChatLogHook));
            await Task.Delay(1000, cancellationToken);
        }

        scopedLogger.LogInformation("Hooks initialized");
    }

    private bool OnAddToChatLog(string message, Channel channel)
    {
        return this.addToChatLogHook.Continue(message, channel);
    }
}
