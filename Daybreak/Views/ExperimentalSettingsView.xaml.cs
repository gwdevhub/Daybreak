using Daybreak.Configuration.Options;
using Daybreak.Services.Navigation;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for ExperimentalSettingsView.xaml
/// </summary>
public partial class ExperimentalSettingsView : UserControl
{
    private readonly IViewManager viewManager;
        
    [GenerateDependencyProperty]
    private bool launchAsCurrentUser;
    [GenerateDependencyProperty]
    private bool multiLaunch;
    [GenerateDependencyProperty]
    private bool dynamicBuildLoading;
    [GenerateDependencyProperty]
    public bool downloadIcons;
    [GenerateDependencyProperty]
    public bool focusViewEnabled;
    [GenerateDependencyProperty]
    private double memoryReaderFrequency;
    [GenerateDependencyProperty]
    private bool pathfindingEnabled;

    private readonly ILiveUpdateableOptions<LauncherOptions> launcherOptions;
    private readonly ILiveUpdateableOptions<BrowserOptions> browserOptions;
    private readonly ILiveUpdateableOptions<FocusViewOptions> focusViewOptions;

    public ExperimentalSettingsView(
        IViewManager viewManager,
        ILiveUpdateableOptions<LauncherOptions> launcherOptions,
        ILiveUpdateableOptions<BrowserOptions> browserOptions,
        ILiveUpdateableOptions<FocusViewOptions> focusViewOptions)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.launcherOptions = launcherOptions.ThrowIfNull();
        this.browserOptions = browserOptions.ThrowIfNull();
        this.focusViewOptions = focusViewOptions.ThrowIfNull();
        this.InitializeComponent();
        this.LoadExperimentalSettings();
    }

    private void LoadExperimentalSettings()
    {
        this.MultiLaunch = this.launcherOptions.Value.MultiLaunchSupport;
        this.DynamicBuildLoading = this.browserOptions.Value.DynamicBuildLoading;
        this.LaunchAsCurrentUser = this.launcherOptions.Value.LaunchGuildwarsAsCurrentUser;
        this.DownloadIcons = this.launcherOptions.Value.DownloadIcons;
        this.FocusViewEnabled = this.focusViewOptions.Value.Enabled;
        this.MemoryReaderFrequency = this.focusViewOptions.Value.MemoryReaderFrequency;
        this.PathfindingEnabled = this.focusViewOptions.Value.EnablePathfinding;
    }

    private void SaveExperimentalSettings()
    {
        var launcherOptions = this.launcherOptions.Value;
        var focusViewOptions = this.focusViewOptions.Value;
        var browserOptions = this.browserOptions.Value;
        launcherOptions.MultiLaunchSupport = this.MultiLaunch;
        browserOptions.DynamicBuildLoading = this.DynamicBuildLoading;
        launcherOptions.LaunchGuildwarsAsCurrentUser = this.LaunchAsCurrentUser;
        launcherOptions.DownloadIcons = this.DownloadIcons;
        focusViewOptions.Enabled = this.FocusViewEnabled;
        focusViewOptions.MemoryReaderFrequency = this.MemoryReaderFrequency;
        focusViewOptions.EnablePathfinding = this.PathfindingEnabled;
        this.launcherOptions.UpdateOption();
        this.browserOptions.UpdateOption();
        this.focusViewOptions.UpdateOption();
        this.viewManager.ShowView<LauncherView>();
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        this.SaveExperimentalSettings();
    }

    private void TextBox_AllowNumbersOnly(object sender, TextCompositionEventArgs e)
    {
        e.Handled = e.Text.Select(char.IsDigit).All(result => result is true) is false;
    }

    private void TextBox_DisallowPaste(object sender, CanExecuteRoutedEventArgs e)
    {
        if (e.Command == ApplicationCommands.ContextMenu || e.Command == ApplicationCommands.Paste)
        {
            e.CanExecute = false;
            e.Handled = true;
        }
    }

    private void MemoryReaderLatencyBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (this.MemoryReaderFrequency < 16)
        {
            this.MemoryReaderFrequency = 16;
        }
        else if (this.MemoryReaderFrequency > 1000)
        {
            this.MemoryReaderFrequency = 1000;
        }
    }
}
