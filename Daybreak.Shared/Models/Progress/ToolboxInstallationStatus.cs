namespace Daybreak.Shared.Models.Progress;
public sealed class ToolboxInstallationStatus : DownloadStatus
{
    public static readonly LoadStatus StartingStep = new ToolboxInstallationStep("Starting");
    public static readonly LoadStatus Finished = new ToolboxInstallationStep("Installation has finished");

    public ToolboxInstallationStatus()
    {
        this.CurrentStep = StartingStep;
    }

    public sealed class ToolboxInstallationStep : LoadStatus
    {
        internal ToolboxInstallationStep(string name) : base(name)
        {
        }
    }
}
