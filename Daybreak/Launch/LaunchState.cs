using Daybreak.Shared.Models.Async;

namespace Daybreak.Launch;

public sealed class LaunchState
{
    public static readonly ProgressUpdate Initializing = new(0, "Initializing");
    public static readonly ProgressUpdate LoadingOptions = new(0.1, "Loading options");

    public static event EventHandler<ProgressUpdate>? ProgressChanged;

    public static ProgressUpdate Progress { get; private set; } = Initializing;

    public static void UpdateProgress(ProgressUpdate progress)
    {
        Progress = progress;
    }
}
