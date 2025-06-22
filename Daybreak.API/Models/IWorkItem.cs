namespace Daybreak.API.Models;

public interface IWorkItem
{
    CancellationToken CancellationToken { get; }
    void Execute();
    void Cancel();
    void Exception(Exception ex);
}
