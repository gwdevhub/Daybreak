using Daybreak.Shared.Models.Async;

namespace Daybreak.Launch;

public sealed class LaunchState
{
    public static readonly ProgressUpdate Initializing = new(0, "Initializing");
    public static ProgressUpdate LoadingServices(double progress) => new(progress, "Loading services");
    public static ProgressUpdate LoadingOptions(double progress) => new(progress, "Loading options");
    public static ProgressUpdate LoadingViews(double progress) => new(progress, "Loading views");
    public static ProgressUpdate LoadingMods(double progress) => new(progress, "Loading mods");
    public static ProgressUpdate LoadingStartupActions(double progress) => new(progress, "Loading startup actions");
    public static ProgressUpdate LoadingPostUpdateActions(double progress) => new(progress, "Loading post-update actions");
    public static ProgressUpdate LoadingNotificationHandlers(double progress) => new(progress, "Loading notification handlers");
    public static ProgressUpdate LoadingArgumentHandlers(double progress) => new(progress, "Loading argument handlers");
    public static ProgressUpdate LoadingThemes(double progress) => new(progress, "Loading themes");
    public static ProgressUpdate LoadingMenuEntries(double progress) => new(progress, "Loading menu entries");
    public static readonly ProgressUpdate ExecutingArgumentHandlers = new(0.9, "Executing argument handlers");
    public static readonly ProgressUpdate Finalizing = new(1.0, "Finalizing");

    public static event EventHandler<ProgressUpdate>? ProgressChanged;

    public static ProgressUpdate Progress { get; private set; } = Initializing;

    public static void UpdateProgress(ProgressUpdate progress)
    {
        Progress = progress;
    }
}
