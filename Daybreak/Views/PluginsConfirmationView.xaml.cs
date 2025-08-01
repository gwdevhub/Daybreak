using Daybreak.Shared.Services.ApplicationLauncher;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views;
/// <summary>
/// Interaction logic for PluginConfirmationView.xaml
/// </summary>
public partial class PluginsConfirmationView : UserControl
{
    //private readonly IViewManager viewManager;
    private readonly IApplicationLauncher applicationLauncher;

    public PluginsConfirmationView(
        //IViewManager viewManager,
        IApplicationLauncher applicationLauncher)
    {
        //this.viewManager = viewManager.ThrowIfNull();
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.InitializeComponent();
    }

    private void YesButton_Clicked(object sender, System.EventArgs e)
    {
        this.applicationLauncher.RestartDaybreak();
    }

    private void NoButton_Clicked(object sender, System.EventArgs e)
    {
        //this.viewManager.ShowView<LauncherView>();
    }
}
