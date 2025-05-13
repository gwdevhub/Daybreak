namespace Daybreak.Services.InternetChecker;

public interface IConnectivityStatus
{
    bool IsInternetAvailable { get; }
}
