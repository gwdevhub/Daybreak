using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.Startup;

public interface IStartupActionProducer
{
    void RegisterAction<T>()
        where T : StartupActionBase;
}
