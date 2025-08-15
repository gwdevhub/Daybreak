using Microsoft.AspNetCore.Components;

namespace Daybreak.Utils;

public static class EventUtil
{
    public static Action AsNonRenderingEventHandler(Action callback) => new SyncReceiver(callback).Invoke;
    public static Action<TValue> AsNonRenderingEventHandler<TValue>(Action<TValue> callback) => new SyncReceiver<TValue>(callback).Invoke;
    public static Func<Task> AsNonRenderingEventHandler(Func<Task> callback) => new AsyncReceiver(callback).Invoke;
    public static Func<TValue, Task> AsNonRenderingEventHandler<TValue>(Func<TValue, Task> callback) => new AsyncReceiver<TValue>(callback).Invoke;

    private record SyncReceiver(Action Callback) : ReceiverBase
    { 
        public void Invoke() => this.Callback();
    }

    private record SyncReceiver<T>(Action<T> Callback) : ReceiverBase
    {
        public void Invoke(T arg) => this.Callback(arg);
    }

    private record AsyncReceiver(Func<Task> Callback) : ReceiverBase
    {
        public Task Invoke() => this.Callback();
    }

    private record AsyncReceiver<T>(Func<T, Task> Callback) : ReceiverBase
    {
        public Task Invoke(T arg) => this.Callback(arg);
    }

    private record ReceiverBase : IHandleEvent
    {
        public Task HandleEventAsync(EventCallbackWorkItem item, object? arg) => item.InvokeAsync(arg);
    }
}
