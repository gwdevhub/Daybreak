using Daybreak.Services.ApplicationArguments.ArgumentHandling;

namespace Daybreak.Services.ApplicationArguments;

public interface IArgumentHandlerProducer
{
    void RegisterArgumentHandler<T>() where
        T : class, IArgumentHandler;
}
