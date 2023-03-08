using System.ComponentModel;

namespace Daybreak.Models.Progress;

public sealed class DownloadStatus : INotifyPropertyChanged
{
    public static readonly DownloadStep StartingStep = new("Starting");
    public static readonly DownloadStep InitializingDownload = new("Initializing download");
    public static DownloadStep Downloading(double progress) => new DownloadProgressStep("Downloading", progress);
    public static readonly DownloadStep DownloadCancelled = new("Download has been canceled");
    public static readonly DownloadStep DownloadFinished = new("Download finished. Starting installer");
    public static readonly DownloadStep Installing = new("Installer is running. Waiting for installer to finish");
    public static readonly DownloadStep Finished = new("Installation has finished");
    public static readonly DownloadStep FailedDownload = new("Download failed. Please check logs for details");

    private DownloadStep currentStep = StartingStep;

    public event PropertyChangedEventHandler? PropertyChanged;

    public DownloadStep CurrentStep
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
        public DownloadStep(string name) : base(name)
        {
        }
    }
    public class DownloadProgressStep : DownloadStep
    {
        internal DownloadProgressStep(string name, double progress) : base(name)
        {
            this.Progress = progress;
        }
    }
}
