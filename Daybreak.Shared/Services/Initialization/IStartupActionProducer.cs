using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.Initialization;

public interface IStartupActionProducer
{
    void RegisterAction<T>()
        where T : StartupActionBase;
}
