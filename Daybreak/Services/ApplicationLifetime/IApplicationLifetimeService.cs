namespace Daybreak.Services.ApplicationLifetime
{
    public interface IApplicationLifetimeService
    {
        void OnStartup();
        void OnClosing();
    }
}
