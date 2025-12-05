using System.Runtime.CompilerServices;

namespace Daybreak.Shared.Models.Async;
public interface IProgressAsyncOperation<T>
{
    event EventHandler<ProgressUpdate>? ProgressChanged;
    TaskAwaiter<T> GetAwaiter();
}
