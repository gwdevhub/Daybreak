using Daybreak.API.Interop;
using Daybreak.API.Models;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Security;

namespace Daybreak.API.Services.Interop;

public sealed class GameThreadService
{
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GameThreadCallback();

    // Prevent GC of delegates passed to native code
    private readonly ConcurrentDictionary<GameThreadCallback, bool> prevent_GC_callbacks = [];

    public Task QueueOnGameThread(Action action, CancellationToken cancellationToken, bool forceEnqueue = false)
    {
        var taskCompletionSource = new TaskCompletionSource();
        var item = new WorkItem(action, taskCompletionSource, cancellationToken);

        GameThreadCallback callback = null!;
        callback = () =>
        {
            try
            {
                if (item.CancellationToken.IsCancellationRequested)
                {
                    item.Cancel();
                    return;
                }

                item.Execute();
            }
            catch (Exception ex)
            {
                item.Exception(ex);
            }
            finally
            {
                this.prevent_GC_callbacks.TryRemove(callback, out _);
            }
        };

        // CRITICAL: Keep delegate alive until callback executes
        this.prevent_GC_callbacks[callback] = true;

        var funcPtr = Marshal.GetFunctionPointerForDelegate(callback);
        GWCA.GW.GameThread.Enqueue(funcPtr, forceEnqueue);

        return taskCompletionSource.Task;
    }

    public Task<T> QueueOnGameThread<T>(Func<T> action, CancellationToken cancellationToken, bool forceEnqueue = false)
    {
        var taskCompletionSource = new TaskCompletionSource<T>();
        var item = new WorkItem<T>(action, taskCompletionSource, cancellationToken);

        GameThreadCallback callback = null!;
        callback = () =>
        {
            try
            {
                if (item.CancellationToken.IsCancellationRequested)
                {
                    item.Cancel();
                    return;
                }

                item.Execute();
            }
            catch (Exception ex)
            {
                item.Exception(ex);
            }
            finally
            {
                // Remove from prevent-GC set after execution
                this.prevent_GC_callbacks.TryRemove(callback, out _);
            }
        };

        // CRITICAL: Keep delegate alive until callback executes
        this.prevent_GC_callbacks[callback] = true;

        var funcPtr = Marshal.GetFunctionPointerForDelegate(callback);
        GWCA.GW.GameThread.Enqueue(funcPtr, forceEnqueue);

        return taskCompletionSource.Task;
    }
}
