using Daybreak.Configuration;
using Daybreak.Services.Navigation;
using Daybreak.Services.Privilege;
using Daybreak.Services.Updater;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Extensions;
using System.Linq;
using System.Windows.Controls;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for AskUpdateView.xaml
/// </summary>
public partial class AskUpdateView : UserControl
{
    private readonly ILogger<AskUpdateView> logger;
    private readonly IViewManager viewManager;
    private readonly ILiveUpdateableOptions<ApplicationConfiguration> liveOptions;
    private readonly IPrivilegeManager privilegeManager;
    private readonly IApplicationUpdater applicationUpdater;

    public AskUpdateView(
        ILogger<AskUpdateView> logger,
        IViewManager viewManager,
        ILiveUpdateableOptions<ApplicationConfiguration> liveOptions,
        IPrivilegeManager privilegeManager,
        IApplicationUpdater applicationUpdater)
    {
        this.logger = logger.ThrowIfNull(nameof(logger));
        this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
        this.liveOptions = liveOptions.ThrowIfNull(nameof(liveOptions));
        this.privilegeManager = privilegeManager.ThrowIfNull(nameof(privilegeManager));
        this.applicationUpdater = applicationUpdater.ThrowIfNull(nameof(applicationUpdater));
        this.InitializeComponent();
    }

    private void NoButton_Clicked(object sender, System.EventArgs e)
    {
        this.logger.LogInformation("User declined update");
        this.liveOptions.Value.AutoCheckUpdate = false;
        this.liveOptions.UpdateOption();
        this.viewManager.ShowView<LauncherView>();
    }

    private async void YesButton_Clicked(object sender, System.EventArgs e)
    {
        this.logger.LogInformation("User accepted update");
        var latestVersion = (await this.applicationUpdater.GetVersions()).Last();
        this.viewManager.ShowView<UpdateView>(latestVersion);
    }
}
