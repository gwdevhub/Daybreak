namespace Daybreak.Models.Progress;
public sealed class GuildwarsInstallationStatus : DownloadStatus
{
    public static readonly LoadStatus StartingStep = new GuildwarsInstallationStep("Starting");
    public static readonly LoadStatus Installing = new GuildwarsInstallationStep("Installer is running. Waiting for installer to finish");
    public static readonly LoadStatus Finished = new GuildwarsInstallationStep("Installation has finished");

    public GuildwarsInstallationStatus()
    {
        this.CurrentStep = StartingStep;
    }

    public sealed class GuildwarsInstallationStep : LoadStatus
    {
        internal GuildwarsInstallationStep(string name) : base(name)
        {
        }
    }
}
