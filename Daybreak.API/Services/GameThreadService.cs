using Daybreak.API.Interop;
using Daybreak.API.Models;
using System.Collections.Concurrent;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using System.Security;

namespace Daybreak.API.Services;

public sealed class GameThreadService
    : IHostedService
{
    private const string LeaveGameThreadFile = "FrApi.cpp";
    private const string LeaveGameThreadAssertion = "renderElapsed >= 0";

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void LeaveGameThread(IntPtr ctx);

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWHook<LeaveGameThread> leaveGameThreadHook;
    private readonly ILogger<GameThreadService> logger;
    private readonly ConcurrentQueue<(TaskCompletionSource, Action)> queuedActions = [];
    private readonly ConcurrentDictionary<Guid, Action> registeredCallbacks = [];

    private CancellationTokenSource? cts = default;

    public GameThreadService(
        MemoryScanningService memoryScanningService,
        ILogger<GameThreadService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.leaveGameThreadHook = new GWHook<LeaveGameThread>(
            new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAssertion(LeaveGameThreadFile, LeaveGameThreadAssertion, 0, 0), 0x300)),
            this.OnLeaveGameThread);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.cts?.Dispose();
        this.cts = new CancellationTokenSource();
        return Task.Factory.StartNew(() => this.InitializeGameThreadHook(this.cts.Token), this.cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.cts?.Cancel();
        this.cts?.Dispose();
        this.leaveGameThreadHook.Dispose();
        return Task.CompletedTask;
    }

    public Task QueueActionOnGameThread(Action action)
    {
        var taskCompletionSource = new TaskCompletionSource();
        this.queuedActions.Enqueue((taskCompletionSource, action));
        return taskCompletionSource.Task;
    }

    public CallbackRegistration RegisterCallback(Action action)
    {
        var uid = new Guid();
        var registration = new CallbackRegistration(uid, () => this.registeredCallbacks.TryRemove(uid, out _));
        this.registeredCallbacks[uid] = action;
        return registration;
    }

    public HookState GetHookState()
    {
        return new HookState
        {
            Hooked = this.leaveGameThreadHook.Hooked,
            TargetAddress = new PointerValue(this.leaveGameThreadHook.TargetAddress),
            ContinueAddress = new PointerValue(this.leaveGameThreadHook.ContinueAddress),
            DetourAddress = new PointerValue(this.leaveGameThreadHook.DetourAddress)
        };
    }

    private async ValueTask InitializeGameThreadHook(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Initializing game thread hook");
        while (!this.leaveGameThreadHook.EnsureInitialized())
        {
            scopedLogger.LogInformation("Waiting for game thread hook to be initialized");
            await Task.Delay(1000, cancellationToken);
        }

        scopedLogger.LogInformation("Game thread hook initialized");
    }

    private void OnLeaveGameThread(IntPtr ctx)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        while(this.queuedActions.TryDequeue(out var tuple))
        {
            try
            {
                tuple.Item2();
                tuple.Item1.TrySetResult();
            }
            catch(Exception ex)
            {
                scopedLogger.LogError(ex, "Error executing action on game thread");
                tuple.Item1.TrySetException(ex);
            }
        }

        foreach(var callback in this.registeredCallbacks)
        {
            try
            {
                callback.Value();
            }
            catch(Exception ex)
            {
                scopedLogger.LogError(ex, "Error executing registered callback {uid}", callback.Key);
            }
        }

        this.leaveGameThreadHook.Continue(ctx);
    }
}
