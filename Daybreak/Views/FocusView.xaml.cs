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
using System.Windows.Media.Animation;

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
    private string browserAddress = string.Empty;

    private CancellationTokenSource? cancellationTokenSource;
    private bool browserMaximized = false;

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

        this.InitializeComponent();

        this.LeftSideBarSize = 25;
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

    private void UpdateGameData()
    {
        if (this.applicationLauncher.IsGuildwarsRunning is false)
        {
            this.logger.LogInformation($"Executable is not running. Returning to {nameof(LauncherView)}");
            this.viewManager.ShowView<LauncherView>();
        }

        if (this.guildwarsMemoryReader.Running is false ||
            this.guildwarsMemoryReader.Faulty)
        {
            this.guildwarsMemoryReader.Initialize(this.applicationLauncher.RunningGuildwarsProcess!);
        }

        this.Dispatcher.Invoke(() =>
        {
            this.GameData = this.guildwarsMemoryReader.GameData;
            if (this.GameData?.MainPlayer is null ||
                this.GameData?.User is null ||
                this.GameData?.Session is null)
            {
                return;
            }

            this.CurrentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(this.GameData.MainPlayer!.Experience);
            this.NextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(this.GameData.MainPlayer!.Experience);
            this.TotalFoes = (int)(this.GameData.Session.FoesKilled + this.GameData.Session.FoesToKill);
            this.Vanquishing = this.GameData.Session.FoesToKill + this.GameData.Session.FoesKilled > 0U;
            this.TitleActive = this.GameData.MainPlayer.TitleInformation is not null && this.GameData.MainPlayer.TitleInformation.IsValid;

            this.MainPlayerDataValid = this.GameData.Valid && this.GameData.MainPlayer.MaxHealth > 0U && this.GameData.MainPlayer.MaxEnergy > 0U;
            if (this.GameData.MainPlayer.TitleInformation is TitleInformation titleInformation && titleInformation.IsValid)
            {
                if (titleInformation.Title is not null)
                {
                    var rankIndex = (int)titleInformation.TierNumber! - 1;
                    this.TitleRankName = titleInformation.Title.Tiers![rankIndex];
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

            this.Browser.Visibility = this.MainPlayerDataValid is true ? Visibility.Visible : Visibility.Collapsed;
            this.UpdateExperienceText();
            this.UpdateLuxonText();
            this.UpdateKurzickText();
            this.UpdateImperialText();
            this.UpdateBalthazarText();
            this.UpdateVanquishingText();
            this.UpdateHealthText();
            this.UpdateEnergyText();
            this.UpdateTitleText();
        },
        System.Windows.Threading.DispatcherPriority.ApplicationIdle);
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
                var currentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(this.GameData.MainPlayer!.Experience);
                var nextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(this.GameData.MainPlayer!.Experience);
                this.ExperienceBarText = $"{(int)currentExperienceInLevel} / {(int)nextLevelExperienceThreshold} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.TotalCurretAndTotalMax:
                var currentTotalExperience = this.GameData.MainPlayer!.Experience;
                var requiredTotalExperience = this.experienceCalculator.GetTotalExperienceForNextLevel(currentTotalExperience);
                this.ExperienceBarText = $"{(int)currentTotalExperience} / {(int)requiredTotalExperience} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.RemainingUntilNextLevel:
                var remainingExperience = this.experienceCalculator.GetRemainingExperienceForNextLevel(this.GameData.MainPlayer!.Experience);
                this.ExperienceBarText = $"Remaining {(int)remainingExperience} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.Percentage:
                var currentExperienceInLevel2 = this.experienceCalculator.GetExperienceForCurrentLevel(this.GameData.MainPlayer!.Experience);
                var nextLevelExperienceThreshold2 = this.experienceCalculator.GetNextExperienceThreshold(this.GameData.MainPlayer!.Experience);
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
                this.VanquishingText = $"{this.GameData.Session!.FoesKilled} / {(int)this.TotalFoes} Foes Killed";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.VanquishingText = $"Remaining {this.GameData.Session!.FoesToKill} Foes";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.VanquishingText = $"{(int)((double)this.GameData.Session!.FoesKilled / (double)this.TotalFoes * 100)}% Foes Killed";
                break;
        }
    }

    private void UpdateHealthText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.HealthDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.HealthBarText = $"{(int)this.GameData.MainPlayer!.CurrentHealth} / {(int)this.GameData.MainPlayer.MaxHealth} Health";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.HealthBarText = $"Remaining {(int)this.GameData.MainPlayer!.CurrentHealth} Health";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.HealthBarText = $"{(int)(this.GameData.MainPlayer!.CurrentHealth / this.GameData.MainPlayer.MaxHealth * 100)}% Health";
                break;
        }
    }

    private void UpdateEnergyText()
    {
        switch (this.liveUpdateableOptions.Value.FocusViewOptions.EnergyDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.EnergyBarText = $"{(int)this.GameData.MainPlayer!.CurrentEnergy} / {(int)this.GameData.MainPlayer.MaxEnergy} Energy";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.EnergyBarText = $"Remaining {(int)this.GameData.MainPlayer!.CurrentEnergy} Energy";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.EnergyBarText = $"{(int)(this.GameData.MainPlayer!.CurrentEnergy / this.GameData.MainPlayer.MaxEnergy * 100)}% Energy";
                break;
        }
    }

    private void UpdateTitleText()
    {
        if (this.GameData.MainPlayer?.TitleInformation?.IsPercentage is true)
        {
            this.TitleText = $"{(double?)this.GameData.MainPlayer?.TitleInformation?.CurrentPoints / 10d}%";
        }
        else
        {
            this.TitleText = $"{this.GameData.MainPlayer?.TitleInformation?.CurrentPoints}/{this.GameData.MainPlayer?.TitleInformation?.PointsForNextRank}";
        }
    }

    private async Task PeriodicallyUpdateGameData(CancellationToken cancellationToken)
    {
        if (this.cancellationTokenSource is null ||
            this.cancellationTokenSource?.IsCancellationRequested is true)
        {
            return;
        }

        this.UpdateGameData();
        await Task.Delay((int)this.liveUpdateableOptions.Value.ExperimentalFeatures.MemoryReaderFrequency, cancellationToken);
        _ = Task.Run(() => this.PeriodicallyUpdateGameData(this.cancellationTokenSource!.Token), this.cancellationTokenSource!.Token);
    }

    private void FocusView_Loaded(object _, RoutedEventArgs e)
    {
        this.UpdateRightSideBarsLayout();
        this.BrowserAddress = this.liveUpdateableOptions.Value.FocusViewOptions.BrowserUrl;
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => this.PeriodicallyUpdateGameData(this.cancellationTokenSource.Token));
    }

    private void FocusView_Unloaded(object _, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = null;
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
        if (this.GameData?.MainPlayer?.PrimaryProfession is not null &&
            this.GameData.MainPlayer.PrimaryProfession != Profession.None)
        {
            this.BrowserAddress = this.GameData.MainPlayer.PrimaryProfession.BuildsUrl;
        }
    }

    private void PrimaryProfession_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.GameData?.MainPlayer?.PrimaryProfession is not null &&
            this.GameData.MainPlayer.PrimaryProfession != Profession.None)
        {
            this.BrowserAddress = this.GameData.MainPlayer.PrimaryProfession.WikiUrl;
        }
    }

    private void SecondaryProfession_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.GameData?.MainPlayer?.SecondaryProfession is not null &&
            this.GameData.MainPlayer.SecondaryProfession != Profession.None)
        {
            this.BrowserAddress = this.GameData.MainPlayer.SecondaryProfession.WikiUrl;
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
            Grid.SetRow(this.Browser, 1);
            Grid.SetColumn(this.Browser, 2);
            Grid.SetRowSpan(this.Browser, 1);
            Grid.SetColumnSpan(this.Browser, 1);
            this.Browser.Margin = new Thickness(10, 0, 0, 0);
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
}
