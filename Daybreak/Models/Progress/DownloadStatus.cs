using System;

namespace Daybreak.Models.Progress;

public abstract class DownloadStatus : ActionStatus
{
    public static readonly LoadStatus InitializingDownload = new DownloadStep("Initializing download");
    public static LoadStatus Downloading(double progress, TimeSpan? eta) => new DownloadProgressStep("Downloading", progress, eta);
    public static readonly LoadStatus DownloadCancelled = new DownloadStep("Download has been cancelled");
    public static readonly LoadStatus DownloadFinished = new DownloadStep("Download finished. Starting installer");
    public static readonly LoadStatus FailedDownload = new DownloadStep("Download failed. Please check logs for details");

    public DownloadStatus()
    {
        this.CurrentStep = InitializingDownload;
    }

    public class DownloadStep : LoadStatus
    {
        internal DownloadStep(string name) : base(name)
        {
        }
    }

    public class DownloadProgressStep : DownloadStep
    {
        public TimeSpan? ETA { get; set; }

        internal DownloadProgressStep(string name, double progress, TimeSpan? eta) : base(name)
        {
            this.Progress = progress;
            this.ETA = eta;
        }
    }
}
