using Daybreak.Services.Configuration;
using Daybreak.Services.ViewManagement;
using Microsoft.Win32;
using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public static readonly DependencyProperty GamePathProperty =
            DependencyPropertyExtensions.Register<SettingsView, string>(nameof(GamePath));
        public static readonly DependencyProperty ToolboxPathProperty =
            DependencyPropertyExtensions.Register<SettingsView, string>(nameof(ToolboxPath));
        public static readonly DependencyProperty AddressBarReadonlyProperty =
            DependencyPropertyExtensions.Register<SettingsView, bool>(nameof(AddressBarReadonly));
        public static readonly DependencyProperty LeftBrowserUrlProperty =
            DependencyPropertyExtensions.Register<SettingsView, string>(nameof(LeftBrowserUrl));
        public static readonly DependencyProperty RightBrowserUrlProperty =
            DependencyPropertyExtensions.Register<SettingsView, string>(nameof(RightBrowserUrl));

        private readonly IConfigurationManager configurationManager;
        private readonly IViewManager viewManager;

        public string GamePath
        {
            get => this.GetTypedValue<string>(GamePathProperty);
            set => this.SetValue(GamePathProperty, value);
        }
        public string ToolboxPath
        {
            get => this.GetTypedValue<string>(ToolboxPathProperty);
            set => this.SetValue(ToolboxPathProperty, value);
        }
        public bool AddressBarReadonly
        {
            get => this.GetTypedValue<bool>(AddressBarReadonlyProperty);
            set => this.SetValue(AddressBarReadonlyProperty, value);
        }
        public string LeftBrowserUrl
        {
            get => this.GetTypedValue<string>(LeftBrowserUrlProperty);
            set => this.SetValue(LeftBrowserUrlProperty, value);
        }
        public string RightBrowserUrl
        {
            get => this.GetTypedValue<string>(RightBrowserUrlProperty);
            set => this.SetValue(RightBrowserUrlProperty, value);
        }

        public SettingsView(
            IConfigurationManager configurationManager,
            IViewManager viewManager)
        {
            this.configurationManager = configurationManager.ThrowIfNull(nameof(configurationManager));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.InitializeComponent();
            this.LoadSettings();
        }

        private void LoadSettings()
        {
            var config = this.configurationManager.GetConfiguration();
            this.AddressBarReadonly = config.AddressBarReadonly;
            this.GamePath = config.GamePath;
            this.ToolboxPath = config.ToolboxPath;
            this.LeftBrowserUrl = config.LeftBrowserDefault;
            this.RightBrowserUrl = config.RightBrowserDefault;
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            var currentConfig = this.configurationManager.GetConfiguration();
            currentConfig.GamePath = this.GamePath;
            currentConfig.ToolboxPath = this.ToolboxPath;
            currentConfig.AddressBarReadonly = this.AddressBarReadonly;
            currentConfig.LeftBrowserDefault = this.LeftBrowserUrl;
            currentConfig.RightBrowserDefault = this.RightBrowserUrl;
            this.configurationManager.SaveConfiguration(currentConfig);
            this.viewManager.ShowView<SettingsCategoryView>();
        }

        private void GameFilePickerGlyph_Clicked(object sender, EventArgs e)
        {
            var filePicker = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "exe",
                Multiselect = false
            };
            if (filePicker.ShowDialog() is true)
            {
                this.GamePath = filePicker.FileName;
            }
        }

        private void ToolboxFilePickerGlyph_Clicked(object sender, EventArgs e)
        {
            var filePicker = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "exe",
                Multiselect = false
            };
            if (filePicker.ShowDialog() is true)
            {
                this.ToolboxPath = filePicker.FileName;
            }
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.viewManager.ShowView<SettingsCategoryView>();
        }

        private void LeftBrowserUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.LeftBrowserUrl = sender.As<TextBox>().Text;
        }

        private void RightBrowserUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.RightBrowserUrl = sender.As<TextBox>().Text;
        }
    }
}
