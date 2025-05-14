namespace Daybreak.Shared.Services.ApplicationArguments;

public interface IApplicationArgumentService : IArgumentHandlerProducer
{
    void HandleArguments(string[] args);
}
