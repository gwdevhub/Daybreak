using Daybreak.Shared.Models.Async;

namespace Daybreak.Launch;

public sealed class LaunchState
{
    public readonly static LaunchState Instance = new();

    public ProgressUpdate Progress;

    private LaunchState()
    {
    }
}
