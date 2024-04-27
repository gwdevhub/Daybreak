namespace Daybreak.Services.ApplicationArguments.ArgumentHandling;
public interface IArgumentHandler
{
    string Identifier { get; }
    int ExpectedArgumentCount { get; }

    void HandleArguments(string[] args);
}
