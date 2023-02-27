using Daybreak.Models;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Navigation;
using Daybreak.Services.Scanner;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for FocusView.xaml
/// </summary>
public partial class FocusView : UserControl
{
    private readonly IApplicationLauncher applicationLauncher;
    private readonly IGuildwarsMemoryReader guildwarsMemoryReader;
    private readonly IViewManager viewManager;
    private readonly ILogger<FocusView> logger;

    [GenerateDependencyProperty]
    private GameData gameData;

    private CancellationTokenSource? cancellationTokenSource;

    public FocusView(
        IApplicationLauncher applicationLauncher,
        IGuildwarsMemoryReader guildwarsMemoryReader,
        IViewManager viewManager,
        ILogger<FocusView> logger)
    {
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.gameData = new GameData();

        this.InitializeComponent();

        this.InitializeBrowser();
    }

    private async void InitializeBrowser()
    {
        await this.Browser.InitializeDefaultBrowser();
    }

    private void UpdateGameData()
    {
        if (this.applicationLauncher.IsGuildwarsRunning is false)
        {
            this.logger.LogInformation($"Executable is not running. Returning to {nameof(LauncherView)}");
            this.viewManager.ShowView<LauncherView>();
        }

        this.Dispatcher.Invoke(() =>
        {
            this.GameData = this.guildwarsMemoryReader.GameData;
        },
        System.Windows.Threading.DispatcherPriority.ApplicationIdle);
    }

    private void ShowInfoBrowser()
    {
        if (this.Browser.BrowserSupported is true)
        {
            this.Browser.Width = 400;
        }
    }

    private void HideInfoBrowser()
    {
        if (this.Browser.BrowserSupported is true)
        {
            this.Browser.Width = 0;
        }
    }

    private void FocusView_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (!this.guildwarsMemoryReader.Running)
        {
            this.guildwarsMemoryReader.Initialize(this.applicationLauncher.RunningGuildwarsProcess!);
        }
        
        if (this.guildwarsMemoryReader.TargetProcess?.MainModule?.FileName != this.applicationLauncher.RunningGuildwarsProcess?.MainModule?.FileName)
        {
            this.guildwarsMemoryReader.Initialize(this.applicationLauncher.RunningGuildwarsProcess!);
        }

        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = new CancellationTokenSource();
        TaskExtensions.RunPeriodicAsync(this.UpdateGameData, TimeSpan.Zero, TimeSpan.FromSeconds(1), this.cancellationTokenSource.Token);
    }

    private void FocusView_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = null;
    }

    private void Quest_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.ShowInfoBrowser();
        this.Browser.Address = this.GameData?.Quest?.WikiUrl?.ToString()!;
    }

    private void FocusView_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.HideInfoBrowser();
    }
}
