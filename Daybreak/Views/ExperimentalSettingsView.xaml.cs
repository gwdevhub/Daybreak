using Daybreak.Services.Configuration;
using Daybreak.Services.ViewManagement;
using System;
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
        public static readonly DependencyProperty MultiLaunchProperty =
            DependencyPropertyExtensions.Register<ExperimentalSettingsView, bool>(nameof(MultiLaunch));
        public static readonly DependencyProperty GWToolboxLaunchDelayProperty =
            DependencyPropertyExtensions.Register<ExperimentalSettingsView, string>(nameof(GWToolboxLaunchDelay));
        public static readonly DependencyProperty DynamicBuildLoadingProperty =
            DependencyPropertyExtensions.Register<ExperimentalSettingsView, bool>(nameof(DynamicBuildLoading));
        public static readonly DependencyProperty LaunchAsCurrentUserProperty =
            DependencyPropertyExtensions.Register<ExperimentalSettingsView, bool>(nameof(LaunchAsCurrentUser));

        private readonly IViewManager viewManager;
        private readonly IConfigurationManager configurationManager;

        public bool LaunchAsCurrentUser
        {
            get => this.GetTypedValue<bool>(LaunchAsCurrentUserProperty);
            set => this.SetValue(LaunchAsCurrentUserProperty, value);
        }
        public bool MultiLaunch
        {
            get => this.GetTypedValue<bool>(MultiLaunchProperty);
            set => this.SetValue(MultiLaunchProperty, value);
        }
        public string GWToolboxLaunchDelay
        {
            get => this.GetTypedValue<string>(GWToolboxLaunchDelayProperty);
            set => this.SetValue(GWToolboxLaunchDelayProperty, value);
        }
        public bool DynamicBuildLoading
        {
            get => this.GetTypedValue<bool>(DynamicBuildLoadingProperty);
            set => this.SetValue(DynamicBuildLoadingProperty, value);
        }

        public ExperimentalSettingsView(
            IViewManager viewManager,
            IConfigurationManager configurationManager)
        {
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.configurationManager = configurationManager.ThrowIfNull(nameof(configurationManager));
            this.InitializeComponent();
            this.LoadExperimentalSettings();
        }

        private void LoadExperimentalSettings()
        {
            var config = this.configurationManager.GetConfiguration();
            this.MultiLaunch = config.ExperimentalFeatures.MultiLaunchSupport;
            this.GWToolboxLaunchDelay = config.ExperimentalFeatures.ToolboxAutoLaunchDelay.ToString();
            this.DynamicBuildLoading = config.ExperimentalFeatures.DynamicBuildLoading;
            this.LaunchAsCurrentUser = config.ExperimentalFeatures.LaunchGuildwarsAsCurrentUser;
        }

        private void SaveExperimentalSettings()
        {
            var config = this.configurationManager.GetConfiguration();
            config.ExperimentalFeatures.MultiLaunchSupport = this.MultiLaunch;
            config.ExperimentalFeatures.DynamicBuildLoading = this.DynamicBuildLoading;
            config.ExperimentalFeatures.LaunchGuildwarsAsCurrentUser = this.LaunchAsCurrentUser;
            if (int.TryParse(this.GWToolboxLaunchDelay, out var gwToolboxLaunchDelay))
            {
                config.ExperimentalFeatures.ToolboxAutoLaunchDelay = gwToolboxLaunchDelay;
            }

            this.configurationManager.SaveConfiguration(config);
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
