namespace Daybreak.Services.ApplicationLifetime
{
    public interface IApplicationLifetimeProducer
    {
        void RegisterService<T>() where T : IApplicationLifetimeService;
    }
}
