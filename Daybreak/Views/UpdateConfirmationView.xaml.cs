using Daybreak.Shared.Models.Versioning;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Updater;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;
/// <summary>
/// Interaction logic for UpdateDetailsView.xaml
/// </summary>
public partial class UpdateConfirmationView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly IApplicationUpdater applicationUpdater;

    [GenerateDependencyProperty]
    public Version version = default!;

    [GenerateDependencyProperty]
    public string changeLog = default!;

    public UpdateConfirmationView(
        IViewManager viewManager,
        IApplicationUpdater applicationUpdater)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.applicationUpdater = applicationUpdater.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void UserControl_Loaded(object _, RoutedEventArgs e)
    {
        if (this.DataContext is not Version version)
        {
            return;
        }

        this.Version = version;
        var maybeChangelog = await this.applicationUpdater.GetChangelog(version);
        if (maybeChangelog is string changeLog)
        {
            this.ChangeLog = changeLog.Replace("<br />", string.Empty);
        }
    }

    private void YesButton_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<UpdateView>(this.Version);
    }

    private void NoButton_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<LauncherView>();
    }
}
