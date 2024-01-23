using Daybreak.Controls;
using Daybreak.Models.Browser;
using Microsoft.Web.WebView2.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Core.Extensions;
using System.Linq;

namespace Daybreak.Services.Browser;
public sealed class BrowserHistoryManager : IBrowserHistoryManager, INotifyPropertyChanged
{
    private HashSet<ulong> eventIds = [];
    private bool userInitiated;
    private bool canGoBack;
    private bool canGoForward;
    private ChromiumBrowserWrapper? browserWrapper;

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool CanGoBack
    {
        get => this.canGoBack;
        set
        {
            this.canGoBack = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CanGoBack)));
        }
    }

    public bool CanGoForward
    {
        get => this.canGoForward;
        set
        {
            this.canGoForward = value;
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
        this.browserWrapper.WebBrowser.NavigationStarting += this.WebView_NavigationStarting;
    }

    public void UnInitializeHistoryManager()
    {
        if (this.browserWrapper is null)
        {
            return;
        }

        this.browserWrapper.WebBrowser.NavigationStarting -= this.WebView_NavigationStarting;
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

        // Skip duplicate commands
        if (this.userInitiated)
        {
            return;
        }

        this.userInitiated = true;
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

        // Skip duplicate commands
        if (this.userInitiated)
        {
            return;
        }

        this.userInitiated = true;
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

    private void WebView_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        if (this.eventIds.Contains(e.NavigationId))
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
            return;
        }

        if (e.NavigationKind is CoreWebView2NavigationKind.Reload)
        {
            return;
        }
        else if (e.NavigationKind is CoreWebView2NavigationKind.BackOrForward)
        {
            e.Cancel = true;
            return;
        }
        else if (this.BrowserHistory.CurrentPosition < this.BrowserHistory.History.Count - 1)
        {
            this.BrowserHistory.History.RemoveRange(this.BrowserHistory.CurrentPosition, this.BrowserHistory.History.Count - this.BrowserHistory.CurrentPosition);
        }

        this.BrowserHistory.History.Add(e.Uri);
        this.BrowserHistory.CurrentPosition = this.BrowserHistory.History.Count - 1;

        this.CanGoBack = this.BrowserHistory.CurrentPosition > 0;
        this.CanGoForward = this.BrowserHistory.CurrentPosition < this.BrowserHistory.History.Count - 1;
    }
}
