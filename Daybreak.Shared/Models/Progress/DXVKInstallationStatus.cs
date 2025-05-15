namespace Daybreak.Shared.Models.Progress;
public sealed class DXVKInstallationStatus : DownloadStatus
{
    public static readonly LoadStatus StartingStep = new DXVKInstallationStep("Starting");
    public static readonly LoadStatus ExtractingFiles = new DXVKInstallationStep("Extracting files");
    public static readonly LoadStatus Finished = new DXVKInstallationStep("Installation has finished");

    public DXVKInstallationStatus()
    {
        this.CurrentStep = StartingStep;
    }

    public sealed class DXVKInstallationStep : LoadStatus
    {
        internal DXVKInstallationStep(string name) : base(name)
        {
        }
    }
}
