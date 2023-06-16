﻿using Daybreak.Configuration.Options;
using Daybreak.Launch;
using Daybreak.Models;
using Daybreak.Models.Guildwars;
using Daybreak.Services.Experience;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls.FocusViewComponents;
/// <summary>
/// Interaction logic for PlayerResourcesComponent.xaml
/// </summary>
public partial class PlayerResourcesComponent : UserControl
{
    private const double BarsTotalSize = 116; // Size of the bars on one side of the screen.

    private readonly IExperienceCalculator experienceCalculator;
    private readonly ILiveUpdateableOptions<FocusViewOptions> liveOptions;

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
    private int pointsInCurrentRank;
    [GenerateDependencyProperty]
    private int pointsForNextRank;
    [GenerateDependencyProperty]
    private bool titleActive;
    [GenerateDependencyProperty]
    private string titleText = string.Empty;

    [GenerateDependencyProperty]
    private string healthBarText = string.Empty;
    [GenerateDependencyProperty]
    private string energyBarText = string.Empty;

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

    public PlayerResourcesComponent()
        : this(
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IExperienceCalculator>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILiveUpdateableOptions<FocusViewOptions>>())
    {
    }

    public PlayerResourcesComponent(
        IExperienceCalculator experienceCalculator,
        ILiveUpdateableOptions<FocusViewOptions> liveOptions)
    {
        this.experienceCalculator = experienceCalculator.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.InitializeComponent();

        this.LeftSideBarSize = 25;
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == VanquishingProperty &&
                e.OldValue != e.NewValue)
        {
            this.UpdateRightSideBarsLayout();
        }
        else if (e.Property == TitleActiveProperty &&
            e.OldValue != e.NewValue)
        {
            this.UpdateRightSideBarsLayout();
        }
        else if (e.Property == DataContextProperty)
        {
            this.UpdateGameData();
        }
    }

    private void Control_Loaded(object sender, RoutedEventArgs e)
    {
        this.UpdateRightSideBarsLayout();
    }

    private void ExperienceBar_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        switch (this.liveOptions.Value.ExperienceDisplay)
        {
            case Configuration.FocusView.ExperienceDisplay.CurrentLevelCurrentAndCurrentLevelMax:
                this.liveOptions.Value.ExperienceDisplay = Configuration.FocusView.ExperienceDisplay.TotalCurretAndTotalMax;
                break;
            case Configuration.FocusView.ExperienceDisplay.TotalCurretAndTotalMax:
                this.liveOptions.Value.ExperienceDisplay = Configuration.FocusView.ExperienceDisplay.RemainingUntilNextLevel;
                break;
            case Configuration.FocusView.ExperienceDisplay.RemainingUntilNextLevel:
                this.liveOptions.Value.ExperienceDisplay = Configuration.FocusView.ExperienceDisplay.Percentage;
                break;
            case Configuration.FocusView.ExperienceDisplay.Percentage:
                this.liveOptions.Value.ExperienceDisplay = Configuration.FocusView.ExperienceDisplay.CurrentLevelCurrentAndCurrentLevelMax;
                break;
        }

        this.liveOptions.UpdateOption();
        this.UpdateExperienceText();
    }

    private void LuxonBar_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        switch (this.liveOptions.Value.LuxonPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveOptions.Value.LuxonPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveOptions.Value.LuxonPointsDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveOptions.Value.LuxonPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveOptions.UpdateOption();
        this.UpdateLuxonText();
    }

    private void KurzickBar_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        switch (this.liveOptions.Value.KurzickPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveOptions.Value.KurzickPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveOptions.Value.KurzickPointsDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveOptions.Value.KurzickPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveOptions.UpdateOption();
        this.UpdateKurzickText();
    }

    private void ImperialBar_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        switch (this.liveOptions.Value.ImperialPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveOptions.Value.ImperialPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveOptions.Value.ImperialPointsDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveOptions.Value.ImperialPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveOptions.UpdateOption();
        this.UpdateImperialText();
    }

    private void BalthazarBar_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        switch (this.liveOptions.Value.BalthazarPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveOptions.Value.BalthazarPointsDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveOptions.Value.BalthazarPointsDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveOptions.Value.BalthazarPointsDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveOptions.UpdateOption();
        this.UpdateBalthazarText();
    }

    private void VanquishingBar_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        switch (this.liveOptions.Value.VanquishingDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveOptions.Value.VanquishingDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveOptions.Value.VanquishingDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveOptions.Value.VanquishingDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveOptions.UpdateOption();
        this.UpdateVanquishingText();
    }

    private void HealthBar_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        switch (this.liveOptions.Value.HealthDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveOptions.Value.HealthDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveOptions.Value.HealthDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveOptions.Value.HealthDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveOptions.UpdateOption();
        this.UpdateHealthText();
    }

    private void EnergyBar_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        switch (this.liveOptions.Value.EnergyDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.liveOptions.Value.EnergyDisplay = Configuration.FocusView.PointsDisplay.Remaining;
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.liveOptions.Value.EnergyDisplay = Configuration.FocusView.PointsDisplay.Percentage;
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.liveOptions.Value.EnergyDisplay = Configuration.FocusView.PointsDisplay.CurrentAndMax;
                break;
        }

        this.liveOptions.UpdateOption();
        this.UpdateEnergyText();
    }

    private void UpdateRightSideBarsLayout()
    {
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

    private void UpdateGameData()
    {
        if (this.DataContext is not GameData gameData ||
            gameData.MainPlayer is not MainPlayerInformation)
        {
            return;
        }

        this.CurrentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(gameData.MainPlayer!.Value.Experience);
        this.NextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(gameData.MainPlayer!.Value.Experience);
        this.TotalFoes = (int)(gameData.Session!.Value.FoesKilled + gameData.Session.Value.FoesToKill);
        this.Vanquishing = gameData.Session.Value.FoesToKill + gameData.Session.Value.FoesKilled > 0U;
        this.TitleActive = gameData.MainPlayer.Value.TitleInformation is not null && gameData.MainPlayer.Value.TitleInformation.Value.IsValid;

        if (gameData.MainPlayer.Value.TitleInformation is TitleInformation titleInformation && titleInformation.IsValid)
        {
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

    private void UpdateExperienceText()
    {
        if (this.DataContext is not GameData gameData)
        {
            return;
        }

        switch (this.liveOptions.Value.ExperienceDisplay)
        {
            case Configuration.FocusView.ExperienceDisplay.CurrentLevelCurrentAndCurrentLevelMax:
                var currentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(gameData.MainPlayer!.Value.Experience);
                var nextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(gameData.MainPlayer!.Value.Experience);
                this.ExperienceBarText = $"{(int)currentExperienceInLevel} / {(int)nextLevelExperienceThreshold} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.TotalCurretAndTotalMax:
                var currentTotalExperience = gameData.MainPlayer!.Value.Experience;
                var requiredTotalExperience = this.experienceCalculator.GetTotalExperienceForNextLevel(currentTotalExperience);
                this.ExperienceBarText = $"{(int)currentTotalExperience} / {(int)requiredTotalExperience} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.RemainingUntilNextLevel:
                var remainingExperience = this.experienceCalculator.GetRemainingExperienceForNextLevel(gameData.MainPlayer!.Value.Experience);
                this.ExperienceBarText = $"Remaining {(int)remainingExperience} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.Percentage:
                var currentExperienceInLevel2 = this.experienceCalculator.GetExperienceForCurrentLevel(gameData.MainPlayer!.Value.Experience);
                var nextLevelExperienceThreshold2 = this.experienceCalculator.GetNextExperienceThreshold(gameData.MainPlayer!.Value.Experience);
                this.ExperienceBarText = $"{(int)((double)currentExperienceInLevel2 / (double)nextLevelExperienceThreshold2 * 100)}% XP";
                break;
        }
    }

    private void UpdateLuxonText()
    {
        if (this.DataContext is not GameData gameData)
        {
            return;
        }

        switch (this.liveOptions.Value.LuxonPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.LuxonBarText = $"{gameData.User!.CurrentLuxonPoints} / {gameData.User.MaxLuxonPoints} Luxon Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.LuxonBarText = $"Remaining {gameData.User!.MaxLuxonPoints - gameData.User.CurrentLuxonPoints} Luxon Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.LuxonBarText = $"{(int)((double)gameData.User!.CurrentLuxonPoints / (double)gameData.User.MaxLuxonPoints * 100)}% Luxon Points";
                break;
        }
    }

    private void UpdateKurzickText()
    {
        if (this.DataContext is not GameData gameData)
        {
            return;
        }

        switch (this.liveOptions.Value.KurzickPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.KurzickBarText = $"{gameData.User!.CurrentKurzickPoints} / {gameData.User.MaxKurzickPoints} Kurzick Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.KurzickBarText = $"Remaining {gameData.User!.MaxKurzickPoints - gameData.User.CurrentKurzickPoints} Kurzick Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.KurzickBarText = $"{(int)((double)gameData.User!.CurrentKurzickPoints / (double)gameData.User.MaxKurzickPoints * 100)}% Kurzick Points";
                break;
        }
    }

    private void UpdateImperialText()
    {
        if (this.DataContext is not GameData gameData)
        {
            return;
        }

        switch (this.liveOptions.Value.ImperialPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.ImperialBarText = $"{gameData.User!.CurrentImperialPoints} / {gameData.User.MaxImperialPoints} Imperial Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.ImperialBarText = $"Remaining {gameData.User!.MaxImperialPoints - gameData.User.CurrentImperialPoints} Imperial Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.ImperialBarText = $"{(int)((double)gameData.User!.CurrentImperialPoints / (double)gameData.User.MaxImperialPoints * 100)}% Imperial Points";
                break;
        }
    }

    private void UpdateBalthazarText()
    {
        if (this.DataContext is not GameData gameData)
        {
            return;
        }

        switch (this.liveOptions.Value.BalthazarPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.BalthazarBarText = $"{gameData.User!.CurrentBalthazarPoints} / {gameData.User.MaxBalthazarPoints} Balthazar Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.BalthazarBarText = $"Remaining {gameData.User!.MaxBalthazarPoints - gameData.User.CurrentBalthazarPoints} Balthazar Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.BalthazarBarText = $"{(int)((double)gameData.User!.CurrentBalthazarPoints / (double)gameData.User.MaxBalthazarPoints * 100)}% Balthazar Points";
                break;
        }
    }

    private void UpdateVanquishingText()
    {
        if (this.DataContext is not GameData gameData)
        {
            return;
        }

        switch (this.liveOptions.Value.VanquishingDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.VanquishingText = $"{gameData.Session!.Value.FoesKilled} / {(int)this.TotalFoes} Foes Killed";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.VanquishingText = $"Remaining {gameData.Session!.Value.FoesToKill} Foes";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.VanquishingText = $"{(int)((double)gameData.Session!.Value.FoesKilled / (double)this.TotalFoes * 100)}% Foes Killed";
                break;
        }
    }

    private void UpdateHealthText()
    {
        if (this.DataContext is not GameData gameData)
        {
            return;
        }

        switch (this.liveOptions.Value.HealthDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.HealthBarText = $"{(int)gameData.MainPlayer!.Value.CurrentHealth} / {(int)gameData.MainPlayer.Value.MaxHealth} Health";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.HealthBarText = $"Remaining {(int)gameData.MainPlayer!.Value.CurrentHealth} Health";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.HealthBarText = $"{(int)(gameData.MainPlayer!.Value.CurrentHealth / gameData.MainPlayer.Value.MaxHealth * 100)}% Health";
                break;
        }
    }

    private void UpdateEnergyText()
    {
        if (this.DataContext is not GameData gameData)
        {
            return;
        }

        switch (this.liveOptions.Value.EnergyDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.EnergyBarText = $"{(int)gameData.MainPlayer!.Value.CurrentEnergy} / {(int)gameData.MainPlayer.Value.MaxEnergy} Energy";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.EnergyBarText = $"Remaining {(int)gameData.MainPlayer!.Value.CurrentEnergy} Energy";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.EnergyBarText = $"{(int)(gameData.MainPlayer!.Value.CurrentEnergy / gameData.MainPlayer.Value.MaxEnergy * 100)}% Energy";
                break;
        }
    }

    private void UpdateTitleText()
    {
        if (this.DataContext is not GameData gameData)
        {
            return;
        }

        if (gameData.MainPlayer?.TitleInformation?.IsPercentage is true)
        {
            this.TitleText = $"{(double?)gameData.MainPlayer?.TitleInformation?.CurrentPoints / 10d}% Rank Progress";
        }
        else
        {
            this.TitleText = $"{gameData.MainPlayer?.TitleInformation?.CurrentPoints}/{gameData.MainPlayer?.TitleInformation?.PointsForNextRank} Rank Progress";
        }
    }
}