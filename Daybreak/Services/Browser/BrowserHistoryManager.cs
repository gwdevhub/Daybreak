using Daybreak.Controls;
using Daybreak.Shared.Models.Browser;
using Microsoft.Web.WebView2.Core;
using System.ComponentModel;
using System.Core.Extensions;

namespace Daybreak.Services.Browser;
public sealed class BrowserHistoryManager : IBrowserHistoryManager, INotifyPropertyChanged
{
    private const int MaxBrowserHistory = 20;
    private static readonly TimeSpan DebounceWindow = TimeSpan.FromMilliseconds(100);

    private DateTime lastOperation = DateTime.Now;
    private string lastUrlCache = string.Empty;
    private readonly HashSet<ulong> eventIds = [];
    private DateTime userInitiatedTime = DateTime.MinValue;
    private bool userInitiated;
    private ChromiumBrowserWrapper? browserWrapper;

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool CanGoBack
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CanGoBack)));
        }
    }

    public bool CanGoForward
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CanGoForward)));
        }
    }

    public BrowserHistory BrowserHistory { get; } = new();

    public BrowserHistoryManager()
    {
    }

    public void InitializeHistoryManager(ChromiumBrowserWrapper chromiumBrowserWrapper)
    {
        this.browserWrapper = chromiumBrowserWrapper.ThrowIfNull();
        this.browserWrapper.WebBrowser.NavigationCompleted += this.WebView_NavigationCompleted;
    }

    public void UnInitializeHistoryManager()
    {
        if (this.browserWrapper is null)
        {
            return;
        }

        this.browserWrapper.WebBrowser.NavigationCompleted -= this.WebView_NavigationCompleted;
    }

    public void GoBack()
    {
        if (this.browserWrapper is null)
        {
            return;
        }

        if (!this.CanGoBack)
        {
            return;
        }

        // Skip duplicate commands only if they fall inside the Debounce window
        if (this.userInitiated && DateTime.Now - this.userInitiatedTime < DebounceWindow)
        {
            return;
        }

        this.userInitiated = true;
        this.userInitiatedTime = DateTime.Now;
        this.BrowserHistory.CurrentPosition--;
        var url = this.BrowserHistory.History[this.BrowserHistory.CurrentPosition];
        this.browserWrapper.WebBrowser.CoreWebView2.Navigate(url);
        this.CanGoBack = this.BrowserHistory.CurrentPosition > 0;
        this.CanGoForward = this.BrowserHistory.CurrentPosition < this.BrowserHistory.History.Count - 1;
    }

    public void GoForward()
    {
        if (this.browserWrapper is null)
        {
            return;
        }

        if (!this.CanGoForward)
        {
            return;
        }

        // Skip duplicate commands only if they fall inside the Debounce window
        if (this.userInitiated && DateTime.Now - this.userInitiatedTime < DebounceWindow)
        {
            return;
        }

        this.userInitiated = true;
        this.userInitiatedTime = DateTime.Now;
        this.BrowserHistory.CurrentPosition++;
        var url = this.BrowserHistory.History[this.BrowserHistory.CurrentPosition];
        this.browserWrapper.WebBrowser.CoreWebView2.Navigate(url);
        this.CanGoBack = this.BrowserHistory.CurrentPosition > 0;
        this.CanGoForward = this.BrowserHistory.CurrentPosition < this.BrowserHistory.History.Count - 1;
    }

    public void Reload()
    {
        this.browserWrapper?.WebBrowser.Reload();
    }

    public void SetBrowserHistory(BrowserHistory browserHistory)
    {
        this.BrowserHistory.History = browserHistory.History;
        this.BrowserHistory.CurrentPosition = browserHistory.CurrentPosition;
        this.CanGoBack = this.BrowserHistory.CurrentPosition > 0;
        this.CanGoForward = this.BrowserHistory.CurrentPosition < this.BrowserHistory.History.Count - 1;

        if (this.BrowserHistory.History.Count > 0 &&
            this.browserWrapper is not null)
        {
            var url = this.BrowserHistory.History[this.BrowserHistory.CurrentPosition];
            this.userInitiated = true;
            this.browserWrapper.Address = url;
        }
    }

    public string? GetCurrentAddress()
    {
        return this.BrowserHistory.History.Skip(this.BrowserHistory.CurrentPosition).FirstOrDefault();
    }

    private void WebView_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        if (this.eventIds.Contains(e.NavigationId))
        {
            return;
        }

        // WebView2 sometimes calls navigation twice for the same url. This condition is supposed to handle the duplicate case
        if (this.browserWrapper?.Address == this.lastUrlCache)
        {
            return;
        }

        // WebView2 when going forward will sometimes call navigation to the old url and quickly call navigation to new url. This condition is supposed to handle the duplicate operation
        if (DateTime.Now - this.lastOperation < DebounceWindow)
        {
            return;
        }

        this.eventIds.Add(e.NavigationId);
        // Ignore the navigation since it was initiated by the user to either go back or forward
        if (this.userInitiated is true)
        {
            this.userInitiated = false;
            this.CanGoBack = this.BrowserHistory.CurrentPosition > 0;
            this.CanGoForward = this.BrowserHistory.CurrentPosition < this.BrowserHistory.History.Count - 1;
            this.lastUrlCache = this.browserWrapper?.Address ?? string.Empty;
            this.lastOperation = DateTime.Now;
            return;
        }

        while (this.BrowserHistory.CurrentPosition < this.BrowserHistory.History.Count - 1)
        {
            this.BrowserHistory.History.RemoveAt(this.BrowserHistory.History.Count - 1);
        }

        if (this.BrowserHistory.History.Count >= MaxBrowserHistory)
        {
            this.BrowserHistory.History.RemoveAt(0);
        }

        this.BrowserHistory.History.Add(this.browserWrapper?.Address ?? string.Empty);
        this.BrowserHistory.CurrentPosition = this.BrowserHistory.History.Count - 1;

        this.CanGoBack = this.BrowserHistory.CurrentPosition > 0;
        this.CanGoForward = this.BrowserHistory.CurrentPosition < this.BrowserHistory.History.Count - 1;
        this.lastUrlCache = this.browserWrapper?.Address ?? string.Empty;
        this.lastOperation = DateTime.Now;
    }
}
