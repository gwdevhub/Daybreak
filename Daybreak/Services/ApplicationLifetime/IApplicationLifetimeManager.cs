namespace Daybreak.Services.ApplicationLifetime
{
    public interface IApplicationLifetimeManager : IApplicationLifetimeProducer
    {
        void OnStartup();
        void OnClosing();
    }
}
