using Daybreak.Configuration;
using Daybreak.Models;
using Daybreak.Models.Guildwars;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Experience;
using Daybreak.Services.Navigation;
using Daybreak.Services.Scanner;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Windows;
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
    private readonly IExperienceCalculator experienceCalculator;
    private readonly IViewManager viewManager;
    private readonly ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions;
    private readonly ILogger<FocusView> logger;

    [GenerateDependencyProperty]
    private GameData gameData;

    [GenerateDependencyProperty]
    private double currentExperienceInLevel;
    [GenerateDependencyProperty]
    private double nextLevelExperienceThreshold;
    [GenerateDependencyProperty]
    private string experienceBarText = string.Empty;

    [GenerateDependencyProperty]
    private string luxonBarText = string.Empty;
    [GenerateDependencyProperty]
    private string kurzickBarText = string.Empty;
    [GenerateDependencyProperty]
    private string imperialBarText = string.Empty;
    [GenerateDependencyProperty]
    private string balthazarBarText = string.Empty;

    [GenerateDependencyProperty]
    private string browserAddress = string.Empty;

    private CancellationTokenSource? cancellationTokenSource;
    private bool browserMaximized = false;

    public FocusView(
        IApplicationLauncher applicationLauncher,
        IGuildwarsMemoryReader guildwarsMemoryReader,
        IExperienceCalculator experienceCalculator,
        IViewManager viewManager,
        ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions,
        ILogger<FocusView> logger)
    {
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.experienceCalculator = experienceCalculator.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.gameData = new GameData();

        this.InitializeComponent();

        this.InitializeBrowser();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == BrowserAddressProperty &&
            this.Browser.BrowserEnabled &&
            this.cancellationTokenSource is not null) //CancellationToken is null only when the view is unloaded. Using this to not overwrite the previous browserurl with the uninitialized value of the address.
        {
            this.liveUpdateableOptions.Value.FocusViewOptions.BrowserUrl = this.BrowserAddress;
            this.liveUpdateableOptions.UpdateOption();
        }

        base.OnPropertyChanged(e);
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
            if (this.GameData?.MainPlayer is null)
            {
                return;
            }

            this.CurrentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(this.GameData.MainPlayer!.Experience);
            this.NextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(this.GameData.MainPlayer!.Experience);
            this.UpdateExperienceText();
            this.UpdateLuxonText();
            this.UpdateKurzickText();
            this.UpdateImperialText();
            this.UpdateBalthazarText();
        },
        System.Windows.Threading.DispatcherPriority.ApplicationIdle);
    }

    private void UpdateExperienceText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.EnergyDisplay)
        {
            case Configuration.FocusView.EnergyDisplay.CurrentLevelCurrentAndCurrentLevelMax:
                var currentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(this.GameData.MainPlayer!.Experience);
                var nextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(this.GameData.MainPlayer!.Experience);
                this.ExperienceBarText = $"{currentExperienceInLevel} / {nextLevelExperienceThreshold} XP";
                break;
            case Configuration.FocusView.EnergyDisplay.TotalCurretAndTotalMax:
                var currentTotalExperience = this.GameData.MainPlayer!.Experience;
                var requiredTotalExperience = this.experienceCalculator.GetTotalExperienceForNextLevel(currentTotalExperience);
                this.ExperienceBarText = $"{currentTotalExperience} / {requiredTotalExperience} XP";
                break;
            case Configuration.FocusView.EnergyDisplay.RemainingUntilNextLevel:
                var remainingExperience = this.experienceCalculator.GetRemainingExperienceForNextLevel(this.GameData.MainPlayer!.Experience);
                this.ExperienceBarText = $"Remaining {remainingExperience} XP";
                break;
        }
    }

    private void UpdateLuxonText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.LuxonPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.LuxonBarText = $"{this.GameData.User!.CurrentLuxonPoints} / {this.GameData.User!.MaxLuxonPoints} Luxon Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.LuxonBarText = $"Remaining {this.GameData.User!.MaxLuxonPoints - this.GameData.User!.CurrentLuxonPoints} Luxon Points";
                break;
        }
    }

    private void UpdateKurzickText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.KurzickPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.KurzickBarText = $"{this.GameData.User!.CurrentKurzickPoints} / {this.GameData.User!.MaxKurzickPoints} Kurzick Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.KurzickBarText = $"Remaining {this.GameData.User!.MaxKurzickPoints - this.GameData.User!.CurrentKurzickPoints} Kurzick Points";
                break;
        }
    }

    private void UpdateImperialText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.ImperialPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.ImperialBarText = $"{this.GameData.User!.CurrentImperialPoints} / {this.GameData.User!.MaxImperialPoints} Imperial Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.ImperialBarText = $"Remaining {this.GameData.User!.MaxImperialPoints - this.GameData.User!.CurrentImperialPoints} Imperial Points";
                break;
        }
    }

    private void UpdateBalthazarText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.BalthazarPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.BalthazarBarText = $"{this.GameData.User!.CurrentBalthazarPoints} / {this.GameData.User!.MaxBalthazarPoints} Balthazar Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.BalthazarBarText = $"Remaining {this.GameData.User!.MaxBalthazarPoints - this.GameData.User!.CurrentBalthazarPoints} Balthazar Points";
                break;
        }
    }

    private void FocusView_Loaded(object sender, RoutedEventArgs e)
    {
        this.BrowserAddress = this.liveUpdateableOptions.Value.FocusViewOptions.BrowserUrl;

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

    private void FocusView_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = null;
    }

    private void ExperienceBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.EnergyDisplay)
        {
            case Configuration.FocusView.EnergyDisplay.CurrentLevelCurrentAndCurrentLevelMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.EnergyDisplay = Configuration.FocusView.EnergyDisplay.TotalCurretAndTotalMax;
                break;
            case Configuration.FocusView.EnergyDisplay.TotalCurretAndTotalMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.EnergyDisplay = Configuration.FocusView.EnergyDisplay.RemainingUntilNextLevel;
                break;
            case Configuration.FocusView.EnergyDisplay.RemainingUntilNextLevel:
                this.liveUpdateableOptions.Value.FocusViewOptions.EnergyDisplay = Configuration.FocusView.EnergyDisplay.CurrentLevelCurrentAndCurrentLevelMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateExperienceText();
    }

    private void LuxonBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.LuxonPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.LuxonPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveUpdateableOptions.Value.FocusViewOptions.LuxonPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateLuxonText();
    }

    private void KurzickBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.KurzickPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.KurzickPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveUpdateableOptions.Value.FocusViewOptions.KurzickPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateKurzickText();
    }

    private void ImperialBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.ImperialPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.ImperialPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveUpdateableOptions.Value.FocusViewOptions.ImperialPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateImperialText();
    }

    private void BalthazarBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.BalthazarPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.BalthazarPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveUpdateableOptions.Value.FocusViewOptions.BalthazarPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateBalthazarText();
    }

    private void CurrentQuest_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.GameData.MainPlayer?.Quest?.WikiUrl is string url)
        {
            this.BrowserAddress = url;
        }
    }

    private void Browser_MaximizeClicked(object sender, EventArgs e)
    {
        this.browserMaximized = !this.browserMaximized;
        if (this.browserMaximized)
        {
            Grid.SetRow(this.Browser, 0);
            Grid.SetColumn(this.Browser, 0);
            Grid.SetRowSpan(this.Browser, int.MaxValue);
            Grid.SetColumnSpan(this.Browser, int.MaxValue);
            this.Browser.Margin = new Thickness(0);
        }
        else
        {
            Grid.SetRow(this.Browser, 1);
            Grid.SetColumn(this.Browser, 2);
            Grid.SetRowSpan(this.Browser, 1);
            Grid.SetColumnSpan(this.Browser, 1);
            this.Browser.Margin = new Thickness(10, 0, 0, 0);
        }
    }

    private void QuestLogTemplate_MapClicked(object _, Map e)
    {
        if (e is null)
        {
            return;
        }

        this.BrowserAddress = e.WikiUrl;
    }
    
    private void QuestLogTemplate_QuestClicked(object _, Quest e)
    {
        if (e is null)
        {
            return;
        }

        this.BrowserAddress = e.WikiUrl;
    }
}
