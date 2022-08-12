using Daybreak.Configuration;
using Daybreak.Services.ViewManagement;
using System;
using System.Configuration;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for ExperimentalSettingsView.xaml
    /// </summary>
    public partial class ExperimentalSettingsView : UserControl
    {
        [GenerateDependencyProperty]
        private bool launchAsCurrentUser;
        [GenerateDependencyProperty]
        private bool multiLaunch;
        [GenerateDependencyProperty]
        private bool dynamicBuildLoading;
        [GenerateDependencyProperty]
        private bool macrosEnabled;
        [GenerateDependencyProperty]
        public string gWToolboxLaunchDelay;
        [GenerateDependencyProperty]
        public bool downloadIcons;

        private readonly IViewManager viewManager;
        private readonly ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions;

        public ExperimentalSettingsView(
            IViewManager viewManager,
            ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions)
        {
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull(nameof(liveUpdateableOptions));
            this.InitializeComponent();
            this.LoadExperimentalSettings();
        }

        private void LoadExperimentalSettings()
        {
            var config = this.liveUpdateableOptions.Value;
            this.MultiLaunch = config.ExperimentalFeatures.MultiLaunchSupport;
            this.GWToolboxLaunchDelay = config.ExperimentalFeatures.ToolboxAutoLaunchDelay.ToString();
            this.DynamicBuildLoading = config.ExperimentalFeatures.DynamicBuildLoading;
            this.LaunchAsCurrentUser = config.ExperimentalFeatures.LaunchGuildwarsAsCurrentUser;
            this.MacrosEnabled = config.ExperimentalFeatures.CanInterceptKeys;
            this.DownloadIcons = config.ExperimentalFeatures.DownloadIcons;
        }

        private void SaveExperimentalSettings()
        {
            var config = this.liveUpdateableOptions.Value;
            config.ExperimentalFeatures.MultiLaunchSupport = this.MultiLaunch;
            config.ExperimentalFeatures.DynamicBuildLoading = this.DynamicBuildLoading;
            config.ExperimentalFeatures.LaunchGuildwarsAsCurrentUser = this.LaunchAsCurrentUser;
            config.ExperimentalFeatures.CanInterceptKeys = this.MacrosEnabled;
            config.ExperimentalFeatures.DownloadIcons = this.DownloadIcons;
            if (int.TryParse(this.GWToolboxLaunchDelay, out var gwToolboxLaunchDelay))
            {
                config.ExperimentalFeatures.ToolboxAutoLaunchDelay = gwToolboxLaunchDelay;
            }

            this.liveUpdateableOptions.UpdateOption();
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            this.SaveExperimentalSettings();
            this.viewManager.ShowView<SettingsCategoryView>();
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.viewManager.ShowView<SettingsCategoryView>();
        }

        private void TextBox_AllowNumbersOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = e.Text.Select(c => char.IsDigit(c)).All(result => result is true) is false;
        }

        private void TextBox_DisallowPaste(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.ContextMenu || e.Command == ApplicationCommands.Paste)
            {
                e.CanExecute = false;
                e.Handled = true;
            }
        }
    }
}
