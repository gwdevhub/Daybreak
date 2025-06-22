using Daybreak.Configuration.Options;
using Daybreak.Shared;
using Daybreak.Shared.Models.FocusView;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls.FocusViewComponents;
/// <summary>
/// Interaction logic for VanquishComponent.xaml
/// </summary>
public partial class VanquishComponent : UserControl
{
    private readonly ILiveUpdateableOptions<FocusViewOptions> liveOptions;

    [GenerateDependencyProperty]
    private int totalFoes;
    [GenerateDependencyProperty]
    private string vanquishingText = string.Empty;

    public VanquishComponent()
        : this(Global.GlobalServiceProvider.GetRequiredService<ILiveUpdateableOptions<FocusViewOptions>>())
    {
    }

    public VanquishComponent(
        ILiveUpdateableOptions<FocusViewOptions> liveOptions)
    {
        this.liveOptions = liveOptions;
        this.InitializeComponent();
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not VanquishComponentContext context)
        {
            return;
        }

        this.TotalFoes = (int)(context.FoesKilled + context.FoesToKill);
        this.UpdateVanquishingText();
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

    private void UpdateVanquishingText()
    {
        if (this.DataContext is not VanquishComponentContext context)
        {
            return;
        }

        switch (this.liveOptions.Value.VanquishingDisplay)
        {
            case Configuration.FocusView.PointsDisplay.CurrentAndMax:
                this.VanquishingText = $"{context.FoesKilled} / {(int)this.TotalFoes} Foes Killed";
                break;
            case Configuration.FocusView.PointsDisplay.Remaining:
                this.VanquishingText = $"Remaining {context.FoesToKill} Foes";
                break;
            case Configuration.FocusView.PointsDisplay.Percentage:
                this.VanquishingText = $"{(int)((double)context.FoesKilled / (double)this.TotalFoes * 100)}% Foes Killed";
                break;
        }
    }
}
