using Daybreak.Shared.Services.ApplicationArguments.ArgumentHandling;

namespace Daybreak.Shared.Services.Initialization;

public interface IArgumentHandlerProducer
{
    void RegisterArgumentHandler<T>() where
        T : class, IArgumentHandler;
}
