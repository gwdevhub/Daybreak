namespace Daybreak.Services.ApplicationDetection
{
    public interface IApplicationDetector
    {
        bool IsGuildwarsRunning { get; }
        bool IsToolboxRunning { get; }
        void LaunchGuildwars();
        void LaunchGuildwarsToolbox();
    }
}
