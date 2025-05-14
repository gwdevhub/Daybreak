namespace Daybreak.Shared.Models.Progress;
public sealed class UModInstallationStatus : DownloadStatus
{
    public static readonly LoadStatus StartingStep = new UModInstallationStep("Starting");
    public static readonly LoadStatus NoGuildwarsExecutable = new UModInstallationStep("No selected Guild Wars executable found");
    public static readonly LoadStatus Installing = new UModInstallationStep("Installer is running. Waiting for installer to finish");
    public static readonly LoadStatus Finished = new UModInstallationStep("Installation has finished");

    public UModInstallationStatus()
    {
        this.CurrentStep = StartingStep;
    }

    public sealed class UModInstallationStep : LoadStatus
    {
        internal UModInstallationStep(string name) : base(name)
        {
        }
    }
}
