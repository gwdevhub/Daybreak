using System;

namespace Daybreak.Models.Progress;
public sealed class GuildwarsInstallationStatus : DownloadStatus
{
    public static readonly LoadStatus StartingStep = new GuildwarsInstallationStep("Starting");
    public static readonly LoadStatus Finished = new GuildwarsInstallationStep("Installation has finished. The new file has been added to the executable list");
    public static readonly LoadStatus StartingExecutable = new GuildwarsInstallationStep("Starting Guildwars. Finish the installation process and close the installer");
    public static DownloadProgressStep Unpacking(double progress, TimeSpan? eta) => new("Unpacking", progress, eta);

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
