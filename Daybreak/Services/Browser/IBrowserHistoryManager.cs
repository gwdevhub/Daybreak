using Daybreak.Controls;
using Daybreak.Models.Browser;

namespace Daybreak.Services.Browser;
public interface IBrowserHistoryManager
{
    bool CanGoBack { get; }
    bool CanGoForward { get; }
    BrowserHistory BrowserHistory { get; }

    void SetBrowserHistory(BrowserHistory browserHistory);
    void InitializeHistoryManager(ChromiumBrowserWrapper chromiumBrowserWrapper);
    void UnInitializeHistoryManager();
    void GoBack();
    void GoForward();
    string? GetCurrentAddress();
    void Reload();
}
