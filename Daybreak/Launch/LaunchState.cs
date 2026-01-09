using Daybreak.Shared.Models.Async;

namespace Daybreak.Launch;

public sealed class LaunchState
{
    public static readonly ProgressUpdate Initializing = new(0, "Initializing");
    public static readonly ProgressUpdate LoadingServices = new(0.1, "Loading services");
    public static readonly ProgressUpdate LoadingOptions = new(0.2, "Loading options");
    public static readonly ProgressUpdate LoadingViews = new(0.3, "Loading views");
    public static readonly ProgressUpdate LoadingMods = new(0.4, "Loading mods");
    public static readonly ProgressUpdate LoadingStartupActions = new(0.5, "Loading startup actions");
    public static readonly ProgressUpdate LoadingPostUpdateActions = new(0.6, "Loading post-update actions");
    public static readonly ProgressUpdate LoadingNotificationHandlers = new(0.7, "Loading notification handlers");
    public static readonly ProgressUpdate LoadingArgumentHandlers = new(0.8, "Loading argument handlers");
    public static readonly ProgressUpdate LoadingThemes = new(0.9, "Loading themes");
    public static readonly ProgressUpdate LoadingMenuEntries = new(1.0, "Loading menu entries");

    public static event EventHandler<ProgressUpdate>? ProgressChanged;

    public static ProgressUpdate Progress { get; private set; } = Initializing;

    public static void UpdateProgress(ProgressUpdate progress)
    {
        Progress = progress;
    }
}
