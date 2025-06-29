using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.ReShade;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.ReShade;
/// <summary>
/// Interaction logic for ReShadeConfigView.xaml
/// </summary>
public partial class ReShadeConfigView : UserControl
{
    private readonly IReShadeService reShadeService;
    private readonly IViewManager viewManager;

    public ReShadeConfigView(
        IReShadeService reShadeService,
        IViewManager viewManager)
    {
        this.reShadeService = reShadeService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void SaveButton_Clicked(object sender, System.EventArgs e)
    {
        await this.reShadeService.SaveConfig(this.TextEditor.Text, CancellationToken.None);
        this.viewManager.ShowView<LauncherView>();
    }

    private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.TextEditor.Text = await this.reShadeService.GetConfig(CancellationToken.None);
    }
}
