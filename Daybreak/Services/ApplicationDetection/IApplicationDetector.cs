namespace Daybreak.Services.ApplicationDetection
{
    public interface IApplicationDetector
    {
        bool IsGuildwarsRunning { get; }
        void LaunchGuildwars();
    }
}
