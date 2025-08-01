using Daybreak.Shared.Services.ReShade;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.ReShade;
/// <summary>
/// Interaction logic for ReShadeConfigView.xaml
/// </summary>
public partial class ReShadePresetView : UserControl
{
    private readonly IReShadeService reShadeService;
    //private readonly IViewManager viewManager;

    public ReShadePresetView(
        IReShadeService reShadeService)
        //IViewManager viewManager)
    {
        this.reShadeService = reShadeService.ThrowIfNull();
        //this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void SaveButton_Clicked(object sender, System.EventArgs e)
    {
        await this.reShadeService.SavePreset(this.TextEditor.Text, CancellationToken.None);
        //this.viewManager.ShowView<LauncherView>();
    }

    private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.TextEditor.Text = await this.reShadeService.GetPreset(CancellationToken.None);
    }
}
