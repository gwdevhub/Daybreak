using Daybreak.Models;

namespace Daybreak.Services.Startup;

public interface IStartupActionProducer
{
    void RegisterAction<T>()
        where T : StartupActionBase;
}
