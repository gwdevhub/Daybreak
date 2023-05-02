using System.ComponentModel;

namespace Daybreak.Models.Progress;

public abstract class DownloadStatus : INotifyPropertyChanged
{
    public static readonly LoadStatus InitializingDownload = new DownloadStep("Initializing download");
    public static LoadStatus Downloading(double progress) => new DownloadProgressStep("Downloading", progress);
    public static readonly LoadStatus DownloadCancelled = new DownloadStep("Download has been canceled");
    public static readonly LoadStatus DownloadFinished = new DownloadStep("Download finished. Starting installer");
    public static readonly LoadStatus FailedDownload = new DownloadStep("Download failed. Please check logs for details");

    private LoadStatus currentStep = InitializingDownload;

    public event PropertyChangedEventHandler? PropertyChanged;

    public LoadStatus CurrentStep
    {
        get => this.currentStep;
        set
        {
            this.currentStep = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentStep)));
        }
    }

    public class DownloadStep : LoadStatus
    {
        internal DownloadStep(string name) : base(name)
        {
        }
    }

    public sealed class DownloadProgressStep : DownloadStep
    {
        internal DownloadProgressStep(string name, double progress) : base(name)
        {
            this.Progress = progress;
        }
    }
}
