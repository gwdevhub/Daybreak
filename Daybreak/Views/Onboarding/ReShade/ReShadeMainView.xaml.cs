using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.UMod;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.ReShade;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.ReShade;
/// <summary>
/// Interaction logic for ReShadeMainView.xaml
/// </summary>
public partial class ReShadeMainView : UserControl
{
    private const string HomepageLink = "https://reshade.me/";

    private readonly IViewManager viewManager;
    private readonly IReShadeService reShadeService;
    private readonly ILiveOptions<UModOptions> liveOptions;

    [GenerateDependencyProperty]
    private bool reShadeEnabled;

    [GenerateDependencyProperty]
    private bool loading;

    public ObservableCollection<UModEntry> Mods { get; } = [];

    public ReShadeMainView(
        IViewManager viewManager,
        IReShadeService reShadeService,
        ILiveOptions<UModOptions> liveOptions)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.reShadeService = reShadeService.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();

        this.InitializeComponent();

        this.ReShadeEnabled = this.reShadeService.IsEnabled;
    }

    private void OpaqueButtonNo_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<LauncherView>();
    }

    private void OpaqueButtonYes_Clicked(object sender, System.EventArgs e)
    {
        this.reShadeService.IsEnabled = !this.reShadeService.IsEnabled;
        this.viewManager.ShowView<LauncherView>();
    }

    private void Homepage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.viewManager.ShowView<ReShadeBrowserView>(HomepageLink);
    }

    private void Download_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.viewManager.ShowView<ReShadeStockEffectsSelectorView>();
    }

    private async void Import_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.Loading = true;
        var dialog = new OpenFileDialog
        {
            Filter = "Zip Archives (*.zip)|*.zip",
            Multiselect = true
        };

        if (dialog.ShowDialog() is not true)
        {
            this.Loading = false;
            return;
        }

        foreach(var selectedPath in dialog.FileNames)
        {
            try
            {
                await this.reShadeService.InstallPackage(selectedPath, CancellationToken.None);
            }
            catch
            {
            }
        }

        await this.Dispatcher.InvokeAsync(() => this.Loading = false);
    }

    private void Config_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.viewManager.ShowView<ReShadeConfigView>();
    }

    private void Preset_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.viewManager.ShowView<ReShadePresetView>();
    }
}
