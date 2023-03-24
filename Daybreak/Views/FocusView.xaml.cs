using Daybreak.Configuration;
using Daybreak.Models;
using Daybreak.Models.Guildwars;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Experience;
using Daybreak.Services.Navigation;
using Daybreak.Services.Scanner;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for FocusView.xaml
/// </summary>
public partial class FocusView : UserControl
{
    private const double BarsTotalSize = 116; // Size of the bars on one side of the screen.

    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IApplicationLauncher applicationLauncher;
    private readonly IGuildwarsMemoryReader guildwarsMemoryReader;
    private readonly IExperienceCalculator experienceCalculator;
    private readonly IViewManager viewManager;
    private readonly ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions;
    private readonly ILogger<FocusView> logger;

    [GenerateDependencyProperty]
    private GameData gameData;

    [GenerateDependencyProperty]
    private PathingData pathingData;

    [GenerateDependencyProperty]
    private bool mainPlayerDataValid;

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
    private string healthBarText = string.Empty;
    [GenerateDependencyProperty]
    private string energyBarText = string.Empty;

    [GenerateDependencyProperty]
    private bool titleActive;
    [GenerateDependencyProperty]
    private string titleText = string.Empty;
    [GenerateDependencyProperty]
    private string titleRankName = string.Empty;
    [GenerateDependencyProperty]
    private int pointsInCurrentRank;
    [GenerateDependencyProperty]
    private int pointsForNextRank;

    [GenerateDependencyProperty]
    private bool vanquishing;
    [GenerateDependencyProperty]
    private int totalFoes;
    [GenerateDependencyProperty]
    private string vanquishingText = string.Empty;

    [GenerateDependencyProperty]
    private double leftSideBarSize;
    [GenerateDependencyProperty]
    private double rightSideBarSize;
    [GenerateDependencyProperty]
    private bool faultyPathingData;

    [GenerateDependencyProperty]
    private string browserAddress = string.Empty;

    private bool browserMaximized = false;
    private bool minimapMaximized = false;
    private CancellationTokenSource? cancellationTokenSource;

    public FocusView(
        IBuildTemplateManager buildTemplateManager,
        IApplicationLauncher applicationLauncher,
        IGuildwarsMemoryReader guildwarsMemoryReader,
        IExperienceCalculator experienceCalculator,
        IViewManager viewManager,
        ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions,
        ILogger<FocusView> logger)
    {
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.experienceCalculator = experienceCalculator.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.gameData = new GameData();
        this.pathingData = new PathingData();

        this.InitializeComponent();

        this.LeftSideBarSize = 25;
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == BrowserAddressProperty &&
            this.Browser.BrowserEnabled)
        {
            this.liveUpdateableOptions.Value.FocusViewOptions.BrowserUrl = this.BrowserAddress;
            this.liveUpdateableOptions.UpdateOption();
        }
        else if (e.Property == VanquishingProperty &&
                e.OldValue != e.NewValue)
        {
            this.UpdateRightSideBarsLayout();
        }
        else if (e.Property == TitleActiveProperty &&
            e.OldValue != e.NewValue)
        {
            this.UpdateRightSideBarsLayout();
        }

        base.OnPropertyChanged(e);
    }

    private TimeSpan GetMemoryReaderLatency()
    {
        return TimeSpan.FromMilliseconds(this.liveUpdateableOptions.Value.ExperimentalFeatures.MemoryReaderFrequency);
    }

    private async Task UpdatePathingData()
    {
        if (this.applicationLauncher.IsGuildwarsRunning is false)
        {
            this.logger.LogInformation($"Executable is not running. Returning to {nameof(LauncherView)}");
            this.viewManager.ShowView<LauncherView>();
        }

        await this.guildwarsMemoryReader.EnsureInitialized().ConfigureAwait(true);
        
        var maybePathingData = await this.guildwarsMemoryReader.ReadPathingData().ConfigureAwait(true);
        if (maybePathingData is not PathingData pathingData ||
            pathingData.Trapezoids is null ||
            pathingData.Trapezoids.Count == 0)
        {
            return;
        }

        this.PathingData = pathingData;
    }

    private async Task UpdateGameData()
    {
        if (this.applicationLauncher.IsGuildwarsRunning is false)
        {
            this.logger.LogInformation($"Executable is not running. Returning to {nameof(LauncherView)}");
            this.viewManager.ShowView<LauncherView>();
        }

        await this.guildwarsMemoryReader.EnsureInitialized().ConfigureAwait(true);

        var maybeGameData = await this.guildwarsMemoryReader.ReadGameData().ConfigureAwait(true);
        if (maybeGameData is not GameData gameData)
        {
            this.MainPlayerDataValid = false;
            this.Browser.Visibility = this.MainPlayerDataValid is true ? Visibility.Visible : Visibility.Collapsed;
            return;
        }

        if (gameData.MainPlayer is null ||
            gameData.User is null ||
            gameData.Session is null ||
            gameData.Valid is false)
        {
            this.MainPlayerDataValid = false;
            this.Browser.Visibility = this.MainPlayerDataValid is true ? Visibility.Visible : Visibility.Collapsed;
            return;
        }

        this.GameData = gameData;

        var pathingMeta = await this.guildwarsMemoryReader.ReadPathingMetaData();
        if (pathingMeta?.TrapezoidCount != this.PathingData.Trapezoids?.Count)
        {
            await this.UpdatePathingData();
        }

        this.CurrentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(this.GameData.MainPlayer!.Value.Experience);
        this.NextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(this.GameData.MainPlayer!.Value.Experience);
        this.TotalFoes = (int)(this.GameData.Session.Value.FoesKilled + this.GameData.Session.Value.FoesToKill);
        this.Vanquishing = this.GameData.Session.Value.FoesToKill + this.GameData.Session.Value.FoesKilled > 0U;
        this.TitleActive = this.GameData.MainPlayer.Value.TitleInformation is not null && this.GameData.MainPlayer.Value.TitleInformation.Value.IsValid;

        this.MainPlayerDataValid = this.GameData.Valid && this.GameData.MainPlayer.Value.MaxHealth > 0U && this.GameData.MainPlayer.Value.MaxEnergy > 0U;
        if (this.GameData.MainPlayer.Value.TitleInformation is TitleInformation titleInformation && titleInformation.IsValid)
        {
            if (titleInformation.Title is not null &&
                titleInformation.Title.Tiers!.Count > titleInformation.TierNumber - 1)
            {
                var rankIndex = (int)titleInformation.TierNumber! - 1;
                this.TitleRankName = $"{titleInformation.Title.Tiers![rankIndex]} ({titleInformation.TierNumber}/{titleInformation.MaxTierNumber})";
            }
            else
            {
                this.TitleRankName = string.Empty;
            }

            if (titleInformation.MaxTierNumber == titleInformation.TierNumber)
            {
                this.PointsInCurrentRank = (int)titleInformation.CurrentPoints!;
                this.PointsForNextRank = (int)titleInformation.CurrentPoints!;
            }
            else if (titleInformation.IsPercentage is false)
            {
                this.PointsInCurrentRank = (int)((uint)titleInformation.CurrentPoints! - (uint)titleInformation.PointsForCurrentRank!);
                this.PointsForNextRank = (int)((uint)titleInformation.PointsForNextRank! - (uint)titleInformation.PointsForCurrentRank!);
            }
            else
            {
                this.PointsInCurrentRank = (int)(uint)titleInformation.CurrentPoints!;
                this.PointsForNextRank = (int)(uint)titleInformation.PointsForNextRank!;
            }
        }

        this.Browser.Visibility = this.MainPlayerDataValid is true ?
            this.minimapMaximized ?
                Visibility.Hidden :
                Visibility.Visible :
            Visibility.Collapsed;
        this.UpdateExperienceText();
        this.UpdateLuxonText();
        this.UpdateKurzickText();
        this.UpdateImperialText();
        this.UpdateBalthazarText();
        this.UpdateVanquishingText();
        this.UpdateHealthText();
        this.UpdateEnergyText();
        this.UpdateTitleText();
    }

    private void UpdateRightSideBarsLayout()
    {
        // If vanquishing, there's 3 bars, otherwise there's only 2 bars
        var bars = 2;
        if (this.Vanquishing)
        {
            bars += 1;
        }

        if (this.TitleActive)
        {
            bars += 1;
        }
        
        var marginsTotalSize = 4 * bars;
        var finalBarSize = (BarsTotalSize - marginsTotalSize) / bars;
        this.RightSideBarSize = finalBarSize;
    }

    private void UpdateExperienceText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.ExperienceDisplay)
        {
            case Configuration.FocusView.ExperienceDisplay.CurrentLevelCurrentAndCurrentLevelMax:
                var currentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(this.GameData.MainPlayer!.Value.Experience);
                var nextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(this.GameData.MainPlayer!.Value.Experience);
                this.ExperienceBarText = $"{(int)currentExperienceInLevel} / {(int)nextLevelExperienceThreshold} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.TotalCurretAndTotalMax:
                var currentTotalExperience = this.GameData.MainPlayer!.Value.Experience;
                var requiredTotalExperience = this.experienceCalculator.GetTotalExperienceForNextLevel(currentTotalExperience);
                this.ExperienceBarText = $"{(int)currentTotalExperience} / {(int)requiredTotalExperience} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.RemainingUntilNextLevel:
                var remainingExperience = this.experienceCalculator.GetRemainingExperienceForNextLevel(this.GameData.MainPlayer!.Value.Experience);
                this.ExperienceBarText = $"Remaining {(int)remainingExperience} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.Percentage:
                var currentExperienceInLevel2 = this.experienceCalculator.GetExperienceForCurrentLevel(this.GameData.MainPlayer!.Value.Experience);
                var nextLevelExperienceThreshold2 = this.experienceCalculator.GetNextExperienceThreshold(this.GameData.MainPlayer!.Value.Experience);
                this.ExperienceBarText = $"{(int)((double)currentExperienceInLevel2 / (double)nextLevelExperienceThreshold2 * 100)}% XP";
                break;
        }
    }

    private void UpdateLuxonText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.LuxonPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.LuxonBarText = $"{this.GameData.User!.CurrentLuxonPoints} / {this.GameData.User.MaxLuxonPoints} Luxon Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.LuxonBarText = $"Remaining {this.GameData.User!.MaxLuxonPoints - this.GameData.User.CurrentLuxonPoints} Luxon Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.LuxonBarText = $"{(int)((double)this.GameData.User!.CurrentLuxonPoints / (double)this.GameData.User.MaxLuxonPoints * 100)}% Luxon Points";
                break;
        }
    }

    private void UpdateKurzickText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.KurzickPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.KurzickBarText = $"{this.GameData.User!.CurrentKurzickPoints} / {this.GameData.User.MaxKurzickPoints} Kurzick Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.KurzickBarText = $"Remaining {this.GameData.User!.MaxKurzickPoints - this.GameData.User.CurrentKurzickPoints} Kurzick Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.KurzickBarText = $"{(int)((double)this.GameData.User!.CurrentKurzickPoints / (double)this.GameData.User.MaxKurzickPoints * 100)}% Kurzick Points";
                break;
        }
    }

    private void UpdateImperialText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.ImperialPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.ImperialBarText = $"{this.GameData.User!.CurrentImperialPoints} / {this.GameData.User.MaxImperialPoints} Imperial Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.ImperialBarText = $"Remaining {this.GameData.User!.MaxImperialPoints - this.GameData.User.CurrentImperialPoints} Imperial Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.ImperialBarText = $"{(int)((double)this.GameData.User!.CurrentImperialPoints / (double)this.GameData.User.MaxImperialPoints * 100)}% Imperial Points";
                break;
        }
    }

    private void UpdateBalthazarText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.BalthazarPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.BalthazarBarText = $"{this.GameData.User!.CurrentBalthazarPoints} / {this.GameData.User.MaxBalthazarPoints} Balthazar Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.BalthazarBarText = $"Remaining {this.GameData.User!.MaxBalthazarPoints - this.GameData.User.CurrentBalthazarPoints} Balthazar Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.BalthazarBarText = $"{(int)((double)this.GameData.User!.CurrentBalthazarPoints / (double)this.GameData.User.MaxBalthazarPoints * 100)}% Balthazar Points";
                break;
        }
    }

    private void UpdateVanquishingText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.VanquishingDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.VanquishingText = $"{this.GameData.Session!.Value.FoesKilled} / {(int)this.TotalFoes} Foes Killed";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.VanquishingText = $"Remaining {this.GameData.Session!.Value.FoesToKill} Foes";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.VanquishingText = $"{(int)((double)this.GameData.Session!.Value.FoesKilled / (double)this.TotalFoes * 100)}% Foes Killed";
                break;
        }
    }

    private void UpdateHealthText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.HealthDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.HealthBarText = $"{(int)this.GameData.MainPlayer!.Value.CurrentHealth} / {(int)this.GameData.MainPlayer.Value.MaxHealth} Health";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.HealthBarText = $"Remaining {(int)this.GameData.MainPlayer!.Value.CurrentHealth} Health";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.HealthBarText = $"{(int)(this.GameData.MainPlayer!.Value.CurrentHealth / this.GameData.MainPlayer.Value.MaxHealth * 100)}% Health";
                break;
        }
    }

    private void UpdateEnergyText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.EnergyDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.EnergyBarText = $"{(int)this.GameData.MainPlayer!.Value.CurrentEnergy} / {(int)this.GameData.MainPlayer.Value.MaxEnergy} Energy";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.EnergyBarText = $"Remaining {(int)this.GameData.MainPlayer!.Value.CurrentEnergy} Energy";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.EnergyBarText = $"{(int)(this.GameData.MainPlayer!.Value.CurrentEnergy / this.GameData.MainPlayer.Value.MaxEnergy * 100)}% Energy";
                break;
        }
    }

    private void UpdateTitleText()
    {
        if (this.GameData.MainPlayer?.TitleInformation?.IsPercentage is true)
        {
            this.TitleText = $"{(double?)this.GameData.MainPlayer?.TitleInformation?.CurrentPoints / 10d}% Rank Progress";
        }
        else
        {
            this.TitleText = $"{this.GameData.MainPlayer?.TitleInformation?.CurrentPoints}/{this.GameData.MainPlayer?.TitleInformation?.PointsForNextRank} Rank Progress";
        }
    }

    private async void FocusView_Loaded(object _, RoutedEventArgs e)
    {
        this.UpdateRightSideBarsLayout();
        this.BrowserAddress = this.liveUpdateableOptions.Value.FocusViewOptions.BrowserUrl;
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = this.cancellationTokenSource.Token;
        var memoryReaderLatency = this.GetMemoryReaderLatency();
        while (!cancellationToken.IsCancellationRequested)
        {
            await this.UpdateGameData().ConfigureAwait(true);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await Task.Delay(memoryReaderLatency, cancellationToken).ConfigureAwait(true);
        }
    }

    private void FocusView_Unloaded(object _, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.guildwarsMemoryReader?.Stop();
    }

    private void ExperienceBar_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.ExperienceDisplay)
        {
            case Configuration.FocusView.ExperienceDisplay.CurrentLevelCurrentAndCurrentLevelMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.ExperienceDisplay = Configuration.FocusView.ExperienceDisplay.TotalCurretAndTotalMax;
                break;
            case Configuration.FocusView.ExperienceDisplay.TotalCurretAndTotalMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.ExperienceDisplay = Configuration.FocusView.ExperienceDisplay.RemainingUntilNextLevel;
                break;
            case Configuration.FocusView.ExperienceDisplay.RemainingUntilNextLevel:
                this.liveUpdateableOptions.Value.FocusViewOptions.ExperienceDisplay = Configuration.FocusView.ExperienceDisplay.Percentage;
                break;
            case Configuration.FocusView.ExperienceDisplay.Percentage:
                this.liveUpdateableOptions.Value.FocusViewOptions.ExperienceDisplay = Configuration.FocusView.ExperienceDisplay.CurrentLevelCurrentAndCurrentLevelMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateExperienceText();
    }

    private void LuxonBar_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.LuxonPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.LuxonPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveUpdateableOptions.Value.FocusViewOptions.LuxonPointsDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveUpdateableOptions.Value.FocusViewOptions.LuxonPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateLuxonText();
    }

    private void KurzickBar_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.KurzickPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.KurzickPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveUpdateableOptions.Value.FocusViewOptions.KurzickPointsDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveUpdateableOptions.Value.FocusViewOptions.KurzickPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateKurzickText();
    }

    private void ImperialBar_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.ImperialPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.ImperialPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveUpdateableOptions.Value.FocusViewOptions.ImperialPointsDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveUpdateableOptions.Value.FocusViewOptions.ImperialPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateImperialText();
    }

    private void BalthazarBar_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.BalthazarPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.BalthazarPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveUpdateableOptions.Value.FocusViewOptions.BalthazarPointsDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveUpdateableOptions.Value.FocusViewOptions.BalthazarPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateBalthazarText();
    }

    private void VanquishingBar_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.VanquishingDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.VanquishingDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveUpdateableOptions.Value.FocusViewOptions.VanquishingDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveUpdateableOptions.Value.FocusViewOptions.VanquishingDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateVanquishingText();
    }

    private void HealthBar_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.HealthDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.HealthDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveUpdateableOptions.Value.FocusViewOptions.HealthDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveUpdateableOptions.Value.FocusViewOptions.HealthDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateHealthText();
    }

    private void EnergyBar_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.EnergyDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveUpdateableOptions.Value.FocusViewOptions.EnergyDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveUpdateableOptions.Value.FocusViewOptions.EnergyDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveUpdateableOptions.Value.FocusViewOptions.EnergyDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.UpdateEnergyText();
    }

    private void CurrentQuest_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.GameData.MainPlayer?.Quest?.WikiUrl is string url)
        {
            this.BrowserAddress = url;
        }
    }

    private void CurrentMap_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.GameData.Session?.CurrentMap?.WikiUrl is string url)
        {
            this.BrowserAddress = url;
        }
    }

    private void MetaBuilds_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.GameData.MainPlayer?.PrimaryProfession is not null &&
            this.GameData.MainPlayer?.PrimaryProfession != Profession.None)
        {
            this.BrowserAddress = this.GameData.MainPlayer!.Value.PrimaryProfession.BuildsUrl;
        }
    }

    private void PrimaryProfession_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.GameData.MainPlayer?.PrimaryProfession is not null &&
            this.GameData.MainPlayer?.PrimaryProfession != Profession.None)
        {
            this.BrowserAddress = this.GameData.MainPlayer!.Value.PrimaryProfession.WikiUrl;
        }
    }

    private void SecondaryProfession_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.GameData.MainPlayer?.SecondaryProfession is not null &&
            this.GameData.MainPlayer?.SecondaryProfession != Profession.None)
        {
            this.BrowserAddress = this.GameData.MainPlayer!.Value.SecondaryProfession.WikiUrl;
        }
    }

    private void EditBuild_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.GameData.MainPlayer?.CurrentBuild is Build build)
        {
            var buildEntry = this.buildTemplateManager.CreateBuild();
            buildEntry.Build = build;
            this.viewManager.ShowView<BuildTemplateView>(buildEntry);
        }
    }

    private void Title_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.GameData.MainPlayer?.TitleInformation?.Title is Title title)
        {
            this.BrowserAddress = title.WikiUrl;
        }
    }

    private void Browser_MaximizeClicked(object _, EventArgs e)
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
            Grid.SetRow(this.Browser, 2);
            Grid.SetColumn(this.Browser, 2);
            Grid.SetRowSpan(this.Browser, 1);
            Grid.SetColumnSpan(this.Browser, 1);
            this.Browser.Margin = new Thickness(5, 0, 0, 0);
        }
    }

    private void Browser_BuildDecoded(object _, Build e)
    {
        if (e is null)
        {
            return;
        }

        var buildEntry = this.buildTemplateManager.CreateBuild();
        buildEntry.Build = e;
        this.viewManager.ShowView<BuildTemplateView>(buildEntry);
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

    private void GuildwarsMinimap_MaximizeClicked(object sender, EventArgs e)
    {
        this.minimapMaximized = !this.minimapMaximized;
        if (this.minimapMaximized)
        {
            Grid.SetRow(this.Minimap, 0);
            Grid.SetColumn(this.Minimap, 0);
            Grid.SetRowSpan(this.Minimap, int.MaxValue);
            Grid.SetColumnSpan(this.Minimap, int.MaxValue);
            this.Minimap.Margin = new Thickness(0);
            this.Browser.Visibility = Visibility.Hidden;
        }
        else
        {
            Grid.SetRow(this.Minimap, 1);
            Grid.SetColumn(this.Minimap, 2);
            Grid.SetRowSpan(this.Minimap, 1);
            Grid.SetColumnSpan(this.Minimap, 1);
            this.Minimap.Margin = new Thickness(5, 0, 0, 5);
            this.Browser.Visibility = Visibility.Visible;
        }
    }
}
