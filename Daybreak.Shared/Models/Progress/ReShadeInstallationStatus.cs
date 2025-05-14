namespace Daybreak.Shared.Models.Progress;
public sealed class ReShadeInstallationStatus : DownloadStatus
{
    public static readonly LoadStatus StartingStep = new ReShadeInstallationStep("Starting");
    public static readonly LoadStatus RetrievingLatestVersionUrl = new ReShadeInstallationStep("Retrieving latest version url");
    public static readonly LoadStatus Installing = new ReShadeInstallationStep("Installer is running. Waiting for installer to finish");
    public static readonly LoadStatus Finished = new ReShadeInstallationStep("Installation has finished");

    public ReShadeInstallationStatus()
    {
        this.CurrentStep = StartingStep;
    }

    public sealed class ReShadeInstallationStep : LoadStatus
    {
        internal ReShadeInstallationStep(string name) : base(name)
        {
        }
    }
}
