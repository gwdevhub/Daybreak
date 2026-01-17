using Daybreak.API.Interop;
using Daybreak.API.Models;
using System.Collections.Concurrent;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using System.Security;

namespace Daybreak.API.Services.Interop;

public sealed class GameThreadService
    : IHostedService, IHookHealthService
{
    private const string LeaveGameThreadFile = "FrApi.cpp";
    private const string LeaveGameThreadAssertion = "renderElapsed >= 0";

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void LeaveGameThread(nint arg1, nint arg2);

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWHook<LeaveGameThread> leaveGameThreadHook;
    private readonly ILogger<GameThreadService> logger;
    private readonly ConcurrentQueue<IWorkItem> queuedItems = [];
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
        return Task.Factory.StartNew(() => this.InitializeHooks(this.cts.Token), this.cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.cts?.Cancel();
        this.cts?.Dispose();
        this.leaveGameThreadHook.Dispose();
        return Task.CompletedTask;
    }

    public Task QueueOnGameThread(Action action, CancellationToken cancellationToken)
    {
        var taskCompletionSource = new TaskCompletionSource();
        this.queuedItems.Enqueue(new WorkItem(action, taskCompletionSource, cancellationToken));
        return taskCompletionSource.Task;
    }

    public Task<T> QueueOnGameThread<T>(Func<T> action, CancellationToken cancellationToken)
    {
        var taskCompletionSource = new TaskCompletionSource<T>();
        this.queuedItems.Enqueue(new WorkItem<T>(action, taskCompletionSource, cancellationToken));
        return taskCompletionSource.Task;
    }

    public CallbackRegistration RegisterCallback(Action action)
    {
        var uid = new Guid();
        var registration = new CallbackRegistration(uid, () => this.registeredCallbacks.TryRemove(uid, out _));
        this.registeredCallbacks[uid] = action;
        return registration;
    }

    public List<HookState> GetHookStates()
    {
        return
            [
                new HookState
                {
                    Hooked = this.leaveGameThreadHook.Hooked,
                    Name = nameof(this.leaveGameThreadHook),
                    TargetAddress = new PointerValue(this.leaveGameThreadHook.TargetAddress),
                    ContinueAddress = new PointerValue(this.leaveGameThreadHook.ContinueAddress),
                    DetourAddress = new PointerValue(this.leaveGameThreadHook.DetourAddress)
                }
            ];
    }

    private async ValueTask InitializeHooks(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Initializing hooks");
        while (!this.leaveGameThreadHook.EnsureInitialized())
        {
            scopedLogger.LogDebug("Waiting for hook to initialize: {hook}", nameof(this.leaveGameThreadHook));
            await Task.Delay(1000, cancellationToken);
        }

        scopedLogger.LogInformation("Hooks initialized");
    }

    private void OnLeaveGameThread(nint arg1, nint arg2)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        while(this.queuedItems.TryDequeue(out var item))
        {
            try
            {
                if (item.CancellationToken.IsCancellationRequested)
                {
                    item.Cancel();
                    continue;
                }

                item.Execute();
            }
            catch(Exception ex)
            {
                scopedLogger.LogError(ex, "Error executing action on game thread");
                item.Exception(ex);
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

        this.leaveGameThreadHook.Continue(arg1, arg2);
    }
}
