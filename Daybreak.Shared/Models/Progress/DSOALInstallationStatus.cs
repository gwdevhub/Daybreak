namespace Daybreak.Models.Progress;
public sealed class DSOALInstallationStatus : DownloadStatus
{
    public static readonly LoadStatus StartingStep = new DSOALInstallationStep("Starting");
    public static readonly LoadStatus ExtractingFiles = new DSOALInstallationStep("Extracting files");
    public static readonly LoadStatus SetupOpenALFiles = new DSOALInstallationStep("Setting up OpenAL files");
    public static readonly LoadStatus Finished = new DSOALInstallationStep("Installation has finished");

    public DSOALInstallationStatus()
    {
        this.CurrentStep = StartingStep;
    }

    public sealed class DSOALInstallationStep : LoadStatus
    {
        internal DSOALInstallationStep(string name) : base(name)
        {
        }
    }
}
