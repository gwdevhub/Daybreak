namespace Daybreak.Shared.Services.InternetChecker;

public interface IConnectivityStatus
{
    bool IsInternetAvailable { get; }
}
