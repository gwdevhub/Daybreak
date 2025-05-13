namespace Daybreak.Services.ApplicationArguments;

public interface IApplicationArgumentService : IArgumentHandlerProducer
{
    void HandleArguments(string[] args);
}
