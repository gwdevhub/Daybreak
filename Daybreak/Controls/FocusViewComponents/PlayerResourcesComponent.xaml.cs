using Daybreak.Configuration.Options;
using Daybreak.Shared;
using Daybreak.Shared.Models.FocusView;
using Daybreak.Shared.Services.Experience;
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
    private string luxonBarText = string.Empty;
    [GenerateDependencyProperty]
    private string kurzickBarText = string.Empty;
    [GenerateDependencyProperty]
    private string imperialBarText = string.Empty;
    [GenerateDependencyProperty]
    private string balthazarBarText = string.Empty;

    public PlayerResourcesComponent()
        : this(
              Global.GlobalServiceProvider.GetRequiredService<IExperienceCalculator>(),
              Global.GlobalServiceProvider.GetRequiredService<ILiveUpdateableOptions<FocusViewOptions>>())
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

    private void UpdateGameData()
    {
        if (this.DataContext is not PlayerResourcesComponentContext)
        {
            return;
        }

        this.UpdateLuxonText();
        this.UpdateKurzickText();
        this.UpdateImperialText();
        this.UpdateBalthazarText();
    }

    private void UpdateLuxonText()
    {
        if (this.DataContext is not PlayerResourcesComponentContext mainPlayerResourceContext)
        {
            return;
        }

        switch (this.liveOptions.Value.LuxonPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.LuxonBarText = $"{mainPlayerResourceContext.CurrentLuxon} / {mainPlayerResourceContext.MaxLuxon} Luxon Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.LuxonBarText = $"Remaining {mainPlayerResourceContext.MaxLuxon - mainPlayerResourceContext.CurrentLuxon} Luxon Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.LuxonBarText = $"{(int)((double)mainPlayerResourceContext.CurrentLuxon / (double)mainPlayerResourceContext.MaxLuxon * 100)}% Luxon Points";
                break;
        }
    }

    private void UpdateKurzickText()
    {
        if (this.DataContext is not PlayerResourcesComponentContext mainPlayerResourceContext)
        {
            return;
        }

        switch (this.liveOptions.Value.KurzickPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.KurzickBarText = $"{mainPlayerResourceContext.CurrentKurzick} / {mainPlayerResourceContext.MaxKurzick} Kurzick Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.KurzickBarText = $"Remaining {mainPlayerResourceContext.MaxKurzick - mainPlayerResourceContext.CurrentKurzick} Kurzick Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.KurzickBarText = $"{(int)((double)mainPlayerResourceContext.CurrentKurzick / (double)mainPlayerResourceContext.MaxKurzick * 100)}% Kurzick Points";
                break;
        }
    }

    private void UpdateImperialText()
    {
        if (this.DataContext is not PlayerResourcesComponentContext mainPlayerResourceContext)
        {
            return;
        }

        switch (this.liveOptions.Value.ImperialPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.ImperialBarText = $"{mainPlayerResourceContext.CurrentImperial} / {mainPlayerResourceContext.MaxImperial} Imperial Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.ImperialBarText = $"Remaining {mainPlayerResourceContext.MaxImperial - mainPlayerResourceContext.CurrentImperial} Imperial Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.ImperialBarText = $"{(int)((double)mainPlayerResourceContext.CurrentImperial / (double)mainPlayerResourceContext.MaxImperial * 100)}% Imperial Points";
                break;
        }
    }

    private void UpdateBalthazarText()
    {
        if (this.DataContext is not PlayerResourcesComponentContext mainPlayerResourceContext)
        {
            return;
        }

        switch (this.liveOptions.Value.BalthazarPointsDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.BalthazarBarText = $"{mainPlayerResourceContext.CurrentBalthazar} / {mainPlayerResourceContext.MaxBalthazar} Balthazar Points";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.BalthazarBarText = $"Remaining {mainPlayerResourceContext.MaxBalthazar - mainPlayerResourceContext.CurrentBalthazar} Balthazar Points";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.BalthazarBarText = $"{(int)((double)mainPlayerResourceContext.CurrentBalthazar / (double)mainPlayerResourceContext.MaxBalthazar * 100)}% Balthazar Points";
                break;
        }
    }
}
