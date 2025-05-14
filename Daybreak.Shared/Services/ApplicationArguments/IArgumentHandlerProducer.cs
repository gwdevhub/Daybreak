using Daybreak.Shared.Services.ApplicationArguments.ArgumentHandling;

namespace Daybreak.Shared.Services.ApplicationArguments;

public interface IArgumentHandlerProducer
{
    void RegisterArgumentHandler<T>() where
        T : class, IArgumentHandler;
}
