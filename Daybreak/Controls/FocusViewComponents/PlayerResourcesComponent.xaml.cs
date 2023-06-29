using Daybreak.Configuration.Options;
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
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == DataContextProperty)
        {
            this.UpdateGameData();
        }
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

    private void UpdateGameData()
    {
        if (this.DataContext is not MainPlayerResourceContext mainPlayerResourceContext ||
            mainPlayerResourceContext.User?.User is null ||
            mainPlayerResourceContext.Session?.Session is null ||
            mainPlayerResourceContext.Player?.PlayerInformation is null)
        {
            return;
        }

        this.CurrentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(mainPlayerResourceContext.Player.PlayerInformation.Experience);
        this.NextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(mainPlayerResourceContext.Player.PlayerInformation.Experience);
        this.TotalFoes = (int)(mainPlayerResourceContext.Session.Session.FoesKilled + mainPlayerResourceContext.Session.Session.FoesToKill);
        this.Vanquishing = mainPlayerResourceContext.Session.Session.FoesToKill + mainPlayerResourceContext.Session.Session.FoesKilled > 0U;
        this.TitleActive = mainPlayerResourceContext.Player.PlayerInformation.TitleInformation is not null && mainPlayerResourceContext.Player.PlayerInformation.TitleInformation.IsValid;

        if (mainPlayerResourceContext.Player.PlayerInformation.TitleInformation is TitleInformation titleInformation && titleInformation.IsValid)
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
        if (this.DataContext is not MainPlayerResourceContext mainPlayerResourceContext ||
            mainPlayerResourceContext.Player?.PlayerInformation is null)
        {
            return;
        }

        switch (this.liveOptions.Value.ExperienceDisplay)
        {
            case Configuration.FocusView.ExperienceDisplay.CurrentLevelCurrentAndCurrentLevelMax:
                var currentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(mainPlayerResourceContext.Player.PlayerInformation.Experience);
                var nextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(mainPlayerResourceContext.Player.PlayerInformation.Experience);
                this.ExperienceBarText = $"{(int)currentExperienceInLevel} / {(int)nextLevelExperienceThreshold} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.TotalCurretAndTotalMax:
                var currentTotalExperience = mainPlayerResourceContext.Player.PlayerInformation.Experience;
                var requiredTotalExperience = this.experienceCalculator.GetTotalExperienceForNextLevel(currentTotalExperience);
                this.ExperienceBarText = $"{(int)currentTotalExperience} / {(int)requiredTotalExperience} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.RemainingUntilNextLevel:
                var remainingExperience = this.experienceCalculator.GetRemainingExperienceForNextLevel(mainPlayerResourceContext.Player.PlayerInformation.Experience);
                this.ExperienceBarText = $"Remaining {(int)remainingExperience} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.Percentage:
                var currentExperienceInLevel2 = this.experienceCalculator.GetExperienceForCurrentLevel(mainPlayerResourceContext.Player.PlayerInformation.Experience);
                var nextLevelExperienceThreshold2 = this.experienceCalculator.GetNextExperienceThreshold(mainPlayerResourceContext.Player.PlayerInformation.Experience);
                this.ExperienceBarText = $"{(int)((double)currentExperienceInLevel2 / (double)nextLevelExperienceThreshold2 * 100)}% XP";
                break;
        }
    }

    private void UpdateLuxonText()
    {
        if (this.DataContext is not MainPlayerResourceContext mainPlayerResourceContext ||
            mainPlayerResourceContext.User?.User is null)
        {
            return;
        }

        switch (this.liveOptions.Value.LuxonPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.LuxonBarText = $"{mainPlayerResourceContext.User.User.CurrentLuxonPoints} / {mainPlayerResourceContext.User.User.MaxLuxonPoints} Luxon Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.LuxonBarText = $"Remaining {mainPlayerResourceContext.User.User.MaxLuxonPoints - mainPlayerResourceContext.User.User.CurrentLuxonPoints} Luxon Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.LuxonBarText = $"{(int)((double)mainPlayerResourceContext.User.User.CurrentLuxonPoints / (double)mainPlayerResourceContext.User.User.MaxLuxonPoints * 100)}% Luxon Points";
                break;
        }
    }

    private void UpdateKurzickText()
    {
        if (this.DataContext is not MainPlayerResourceContext mainPlayerResourceContext ||
            mainPlayerResourceContext.User?.User is null)
        {
            return;
        }

        switch (this.liveOptions.Value.KurzickPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.KurzickBarText = $"{mainPlayerResourceContext.User.User.CurrentKurzickPoints} / {mainPlayerResourceContext.User.User.MaxKurzickPoints} Kurzick Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.KurzickBarText = $"Remaining {mainPlayerResourceContext.User.User!.MaxKurzickPoints - mainPlayerResourceContext.User.User.CurrentKurzickPoints} Kurzick Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.KurzickBarText = $"{(int)((double)mainPlayerResourceContext.User.User.CurrentKurzickPoints / (double)mainPlayerResourceContext.User.User.MaxKurzickPoints * 100)}% Kurzick Points";
                break;
        }
    }

    private void UpdateImperialText()
    {
        if (this.DataContext is not MainPlayerResourceContext mainPlayerResourceContext ||
            mainPlayerResourceContext.User?.User is null)
        {
            return;
        }

        switch (this.liveOptions.Value.ImperialPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.ImperialBarText = $"{mainPlayerResourceContext.User.User!.CurrentImperialPoints} / {mainPlayerResourceContext.User.User.MaxImperialPoints} Imperial Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.ImperialBarText = $"Remaining {mainPlayerResourceContext.User.User!.MaxImperialPoints - mainPlayerResourceContext.User.User.CurrentImperialPoints} Imperial Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.ImperialBarText = $"{(int)((double)mainPlayerResourceContext.User.User!.CurrentImperialPoints / (double)mainPlayerResourceContext.User.User.MaxImperialPoints * 100)}% Imperial Points";
                break;
        }
    }

    private void UpdateBalthazarText()
    {
        if (this.DataContext is not MainPlayerResourceContext mainPlayerResourceContext ||
            mainPlayerResourceContext.User?.User is null)
        {
            return;
        }

        switch (this.liveOptions.Value.BalthazarPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.BalthazarBarText = $"{mainPlayerResourceContext.User.User.CurrentBalthazarPoints} / {mainPlayerResourceContext.User.User.MaxBalthazarPoints} Balthazar Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.BalthazarBarText = $"Remaining {mainPlayerResourceContext.User.User.MaxBalthazarPoints - mainPlayerResourceContext.User.User.CurrentBalthazarPoints} Balthazar Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.BalthazarBarText = $"{(int)((double)mainPlayerResourceContext.User.User.CurrentBalthazarPoints / (double)mainPlayerResourceContext.User.User.MaxBalthazarPoints * 100)}% Balthazar Points";
                break;
        }
    }

    private void UpdateVanquishingText()
    {
        if (this.DataContext is not MainPlayerResourceContext mainPlayerResourceContext ||
            mainPlayerResourceContext.Session?.Session is null)
        {
            return;
        }

        switch (this.liveOptions.Value.VanquishingDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.VanquishingText = $"{mainPlayerResourceContext.Session.Session.FoesKilled} / {(int)this.TotalFoes} Foes Killed";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.VanquishingText = $"Remaining {mainPlayerResourceContext.Session.Session.FoesToKill} Foes";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.VanquishingText = $"{(int)((double)mainPlayerResourceContext.Session.Session.FoesKilled / (double)this.TotalFoes * 100)}% Foes Killed";
                break;
        }
    }

    private void UpdateHealthText()
    {
        if (this.DataContext is not MainPlayerResourceContext mainPlayerResourceContext ||
            mainPlayerResourceContext.Player?.PlayerInformation is null)
        {
            return;
        }

        switch (this.liveOptions.Value.HealthDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.HealthBarText = $"{(int)mainPlayerResourceContext.Player.PlayerInformation.CurrentHealth} / {(int)mainPlayerResourceContext.Player.PlayerInformation.MaxHealth} Health";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.HealthBarText = $"Remaining {(int)mainPlayerResourceContext.Player.PlayerInformation.CurrentHealth} Health";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.HealthBarText = $"{(int)(mainPlayerResourceContext.Player.PlayerInformation.CurrentHealth / mainPlayerResourceContext.Player.PlayerInformation.MaxHealth * 100)}% Health";
                break;
        }
    }

    private void UpdateEnergyText()
    {
        if (this.DataContext is not MainPlayerResourceContext mainPlayerResourceContext ||
            mainPlayerResourceContext.Player?.PlayerInformation is null)
        {
            return;
        }

        switch (this.liveOptions.Value.EnergyDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.EnergyBarText = $"{(int)mainPlayerResourceContext.Player.PlayerInformation.CurrentEnergy} / {(int)mainPlayerResourceContext.Player.PlayerInformation.MaxEnergy} Energy";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.EnergyBarText = $"Remaining {(int)mainPlayerResourceContext.Player.PlayerInformation.CurrentEnergy} Energy";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.EnergyBarText = $"{(int)(mainPlayerResourceContext.Player.PlayerInformation.CurrentEnergy / mainPlayerResourceContext.Player.PlayerInformation.MaxEnergy * 100)}% Energy";
                break;
        }
    }

    private void UpdateTitleText()
    {
        if (this.DataContext is not MainPlayerResourceContext mainPlayerResourceContext ||
            mainPlayerResourceContext.Player?.PlayerInformation is null)
        {
            return;
        }

        if (mainPlayerResourceContext.Player.PlayerInformation.TitleInformation?.IsPercentage is true)
        {
            this.TitleText = $"{(double?)mainPlayerResourceContext.Player.PlayerInformation.TitleInformation?.CurrentPoints / 10d}% Rank Progress";
        }
        else
        {
            this.TitleText = $"{mainPlayerResourceContext.Player.PlayerInformation.TitleInformation?.CurrentPoints}/{mainPlayerResourceContext.Player.PlayerInformation.TitleInformation?.PointsForNextRank} Rank Progress";
        }
    }
}
