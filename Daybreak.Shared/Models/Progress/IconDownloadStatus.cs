using System.ComponentModel;

namespace Daybreak.Shared.Models.Progress;

public sealed class IconDownloadStatus : INotifyPropertyChanged
{
    public static readonly IconDownloadStep StartingStep = new StartingIconDownloadStep();
    public static readonly IconDownloadStep Finished = new FinishedIconDownloadStep();
    public static readonly IconDownloadStep BrowserNotSupported = new NotSupportedIconDownloadStep();
    public static IconDownloadStep Checking(string iconName, double progress) => new CheckingIconDownloadStep(iconName, progress);
    public static IconDownloadStep Downloading(string iconName, double progress) => new DownloadingIconDownloadStep(iconName, progress);
    public static IconDownloadStep Stopped(double progress) => new StoppedIconDownloadStep(progress);

    public event PropertyChangedEventHandler? PropertyChanged;

    public IconDownloadStep CurrentStep
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentStep)));
        }
    } = StartingStep;

    public abstract class IconDownloadStep : LoadStatus
    {
        public IconDownloadStep(string name, double progress) : base(name)
        {
            this.Progress = progress;
        }
    }

    public class StoppedIconDownloadStep(double progress) : IconDownloadStep("Download stopped", progress)
    {
    }

    public class DownloadingIconDownloadStep(string skillName, double progress) : IconDownloadStep($"Downloading [{skillName}] icon", progress)
    {
    }

    public class CheckingIconDownloadStep(string skillName, double progress) : IconDownloadStep($"Checking [{skillName}] icon", progress)
    {
    }

    public class NotSupportedIconDownloadStep : IconDownloadStep
    {
        public NotSupportedIconDownloadStep() : base("Cannot download icons. The WebView2 browser is not supported", 0d)
        {
        }
    }

    public class FinishedIconDownloadStep : IconDownloadStep
    {
        public FinishedIconDownloadStep() : base("Download finished", 100d)
        {
        }
    }

    public class StartingIconDownloadStep : IconDownloadStep
    {
        public StartingIconDownloadStep() : base("Download starting", 0d)
        {
        }
    }
}
