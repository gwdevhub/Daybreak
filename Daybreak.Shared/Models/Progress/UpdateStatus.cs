namespace Daybreak.Shared.Models.Progress;

public sealed class UpdateStatus : DownloadStatus
{
    public static readonly LoadStatus Starting = new UpdateStep("Starting");
    public static readonly LoadStatus CheckingLatestVersion = new UpdateStep("Checking latest version");
    public static readonly LoadStatus PendingRestart = new UpdateStep("Download finished. Application will restart in order to apply the update");

    public UpdateStatus()
    {
        this.CurrentStep = Starting;
    }

    public class UpdateStep : LoadStatus
    {
        public UpdateStep(string name) : base(name)
        {
        }
    }
}
