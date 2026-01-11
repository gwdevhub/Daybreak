using System.Runtime.CompilerServices;

namespace Daybreak.Shared.Models.Async;

/// <summary>
/// Represents an abstract asynchronous operation that can report progress and be canceled.
/// Constructed using <see cref="ProgressAsyncOperation.Create{T}(Task{T}, CancellationToken)"/>.
/// </summary>
/// <remarks>
/// Services should return <see cref="IProgressAsyncOperation{T}"/> so that consumers can monitor progress and await the result.
/// <see cref="ProgressChanged"/> event is raised whenever progress is reported via the <see cref="IProgress{T}.Report(T)"/> method. The operation can be awaited and will complete when the underlying task completes.
/// </remarks>
public sealed class ProgressAsyncOperation<T> : IProgressAsyncOperation<T>
{
    private readonly Progress<ProgressUpdate> progress = new();
    private readonly Task<T> task;
    private readonly CancellationTokenSource cancellationTokenSource;

    public event EventHandler<ProgressUpdate>? ProgressChanged;
    public ProgressUpdate? CurrentProgress { get; private set; }

    public bool IsCompleted => this.task.IsCompleted;
    public bool IsCanceled => this.task.IsCanceled;
    public bool IsFaulted => this.task.IsFaulted;
    public CancellationToken CancellationToken => this.cancellationTokenSource.Token;

    internal ProgressAsyncOperation(Func<Progress<ProgressUpdate>, Task<T>> wrappedTaskWithProgress, CancellationToken cancellationToken)
    {
        this.progress.ProgressChanged += (s, e) =>
        {
            this.CurrentProgress = e;
            this.ProgressChanged?.Invoke(this, e);
        };
        
        this.task = wrappedTaskWithProgress(this.progress);
        this.cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    }

    public TaskAwaiter<T> GetAwaiter()
    {
        return this.task.GetAwaiter();
    }
}

public static class ProgressAsyncOperation
{
    /// <summary>
    /// Creates a new <see cref="ProgressAsyncOperation{T}"/> instance that wraps the provided task and cancellation token and returns an awaitable that reports progress.
    /// </summary>
    /// <remarks>
    /// Inside the task factory, progress can be reported by calling the <see cref="IProgress{T}.Report(T)"/> method on the provided <see cref="IProgress{ProgressUpdate}"/> instance.
    /// </remarks>
    public static ProgressAsyncOperation<T> Create<T>(Func<IProgress<ProgressUpdate>, Task<T>> wrappedTaskWithProgress, CancellationToken cancellationToken)
    {
        return new ProgressAsyncOperation<T>(wrappedTaskWithProgress, cancellationToken);
    }
}
