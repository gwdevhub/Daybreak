
namespace Daybreak.Models.Progress;
public sealed class DirectSongInstallationStatus : DownloadStatus
{
    public static readonly LoadStatus StartingStep = new DirectSongInstallationStep("Starting");
    public static readonly LoadStatus ExtractingFiles = new DirectSongInstallationStep("Extracting files");
    public static readonly LoadStatus Finished = new DirectSongInstallationStep("Installation has finished");
    public static readonly LoadStatus SetupRegistry = new DirectSongInstallationStep("Setting up registry entries");
    public static LoadStatus Extracting(double progress) => new DirectSongInstallationProgressStep("Extracting DirectSong archive", progress);

    public DirectSongInstallationStatus()
    {
        this.CurrentStep = StartingStep;
    }

    public sealed class DirectSongInstallationStep : LoadStatus
    {
        internal DirectSongInstallationStep(string name) : base(name)
        {
        }
    }

    public sealed class DirectSongInstallationProgressStep : LoadStatus
    {
        internal DirectSongInstallationProgressStep(string name, double progress) : base(name)
        {
            this.Progress = progress;
        }
    }
}
