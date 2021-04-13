using Daybreak.Services.Configuration;
using Daybreak.Services.ViewManagement;
using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for ExperimentalSettingsView.xaml
    /// </summary>
    public partial class ExperimentalSettingsView : UserControl
    {
        public static readonly DependencyProperty MultiLaunchProperty = DependencyPropertyExtensions.Register<ExperimentalSettingsView, bool>(nameof(MultiLaunch));

        private readonly IViewManager viewManager;
        private readonly IConfigurationManager configurationManager;

        public bool MultiLaunch
        {
            get => this.GetTypedValue<bool>(MultiLaunchProperty);
            set => this.SetValue(MultiLaunchProperty, value);
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
        }

        private void SaveExperimentalSettings()
        {
            var config = this.configurationManager.GetConfiguration();
            config.ExperimentalFeatures.MultiLaunchSupport = this.MultiLaunch;
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
    }
}
