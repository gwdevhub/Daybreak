using Daybreak.Configuration.Options;
using Daybreak.Shared;
using Daybreak.Shared.Models.FocusView;
using Daybreak.Shared.Services.Experience;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Core.Extensions;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls.FocusViewComponents;

public partial class CharacterComponent : UserControl
{
    private readonly IExperienceCalculator experienceCalculator;
    private readonly ILiveUpdateableOptions<FocusViewOptions> liveOptions;

    public event EventHandler<string>? NavigateToClicked;
    public event EventHandler<CharacterSelectComponentEntry>? SwitchCharacterClicked;

    public ObservableCollection<CharacterSelectComponentEntry> Characters { get; } = [];
    
    [GenerateDependencyProperty]
    private CharacterSelectComponentEntry currentCharacter = default!;

    [GenerateDependencyProperty]
    private double currentExperienceInLevel;
    [GenerateDependencyProperty]
    private double nextLevelExperienceThreshold;
    [GenerateDependencyProperty]
    private string experienceBarText = string.Empty;

    private uint experienceCache = 0;

    public CharacterComponent()
        :this(Global.GlobalServiceProvider.GetRequiredService<IExperienceCalculator>(),
              Global.GlobalServiceProvider.GetRequiredService<ILiveUpdateableOptions<FocusViewOptions>>())
    {
    }

    public CharacterComponent(
        IExperienceCalculator experienceCalculator,
        ILiveUpdateableOptions<FocusViewOptions> liveOptions)
    {
        this.experienceCalculator = experienceCalculator.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.InitializeComponent();
    }

    private void UserControl_DataContextChanged(object _, System.Windows.DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not CharacterComponentContext context)
        {
            return;
        }

        if (this.CurrentCharacter != context.CurrentCharacter)
        {
            this.CurrentCharacter = context.CurrentCharacter;
        }

        var charsToAdd = context.Characters
            .Where(c => !this.Characters.Contains(c))
            .ToList();
        var charsToRemove = this.Characters
            .Where(c => !context.Characters.Contains(c))
            .ToList();

        foreach (var character in charsToAdd)
        {
            this.Characters.Add(character);
        }

        foreach (var character in charsToRemove)
        {
            this.Characters.Remove(character);
        }

        if (this.experienceCache != context.CurrentExperience)
        {
            this.experienceCache = context.CurrentExperience;
            this.CurrentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(context.CurrentExperience);
            this.NextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(context.CurrentExperience);
            this.UpdateExperienceText();
        }
    }

    private void DropDownButton_SelectionChanged(object _, object newEntry)
    {
        if (newEntry is not CharacterSelectComponentEntry newCharacter)
        {
            return;
        }

        this.SwitchCharacterClicked?.Invoke(this, newCharacter);
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

    private void UpdateExperienceText()
    {
        if (this.DataContext is not CharacterComponentContext context)
        {
            return;
        }

        switch (this.liveOptions.Value.ExperienceDisplay)
        {
            case Configuration.FocusView.ExperienceDisplay.CurrentLevelCurrentAndCurrentLevelMax:
                var currentExperienceInLevel = this.experienceCalculator.GetExperienceForCurrentLevel(context.CurrentExperience);
                var nextLevelExperienceThreshold = this.experienceCalculator.GetNextExperienceThreshold(context.CurrentExperience);
                this.ExperienceBarText = $"{(int)currentExperienceInLevel} / {(int)nextLevelExperienceThreshold} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.TotalCurretAndTotalMax:
                var currentTotalExperience = context.CurrentExperience;
                var requiredTotalExperience = this.experienceCalculator.GetTotalExperienceForNextLevel(currentTotalExperience);
                this.ExperienceBarText = $"{(int)currentTotalExperience} / {(int)requiredTotalExperience} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.RemainingUntilNextLevel:
                var remainingExperience = this.experienceCalculator.GetRemainingExperienceForNextLevel(context.CurrentExperience);
                this.ExperienceBarText = $"Remaining {(int)remainingExperience} XP";
                break;
            case Configuration.FocusView.ExperienceDisplay.Percentage:
                var currentExperienceInLevel2 = this.experienceCalculator.GetExperienceForCurrentLevel(context.CurrentExperience);
                var nextLevelExperienceThreshold2 = this.experienceCalculator.GetNextExperienceThreshold(context.CurrentExperience);
                this.ExperienceBarText = $"{(int)((double)currentExperienceInLevel2 / (double)nextLevelExperienceThreshold2 * 100)}% XP";
                break;
        }
    }
}
