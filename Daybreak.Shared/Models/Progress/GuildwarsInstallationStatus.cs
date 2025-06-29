namespace Daybreak.Shared.Models.Progress;
public sealed class GuildwarsInstallationStatus : DownloadStatus
{
    public static readonly LoadStatus StartingStep = new GuildwarsInstallationStep("Starting");
    public static readonly LoadStatus InstallFinished = new GuildwarsInstallationStep("Installation has finished. The new file has been added to the executable list", true);
    public static readonly LoadStatus StartingExecutable = new GuildwarsInstallationStep("Starting Guildwars. Finish the installation process and close the installer");
    public static readonly LoadStatus UpdateFinished = new GuildwarsInstallationStep("Update has finished", true);
    public static readonly LoadStatus Failed = new GuildwarsInstallationStep("Operation failed. Please check logs for details", true);
    public static UnpackingProgressStep Unpacking(double progress, TimeSpan? eta) => new(progress, eta);

    public GuildwarsInstallationStatus()
    {
        this.CurrentStep = StartingStep;
    }

    public sealed class GuildwarsInstallationStep : LoadStatus
    {
        public bool Final { get; init; } = false;

        internal GuildwarsInstallationStep(string name, bool final = false) : base(name)
        {
            this.Final = final;
        }
    }

    public sealed class UnpackingProgressStep : DownloadProgressStep
    {
        public UnpackingProgressStep(double progress, TimeSpan? eta)
            : base("Unpacking", progress, eta)
        {
        }
    }
}
