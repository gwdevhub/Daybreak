﻿using Daybreak.Configuration;
using Daybreak.Services.ViewManagement;
using System;
using System.Configuration;
using System.Extensions;
using System.Windows.Extensions;
using System.Windows.Forms;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
    public partial class SettingsView : System.Windows.Controls.UserControl
    {
        private readonly ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions;
        private readonly IViewManager viewManager;

        [GenerateDependencyProperty]
        private string texmodPath;
        [GenerateDependencyProperty]
        private bool toolboxAutoLaunch;
        [GenerateDependencyProperty]
        private string toolboxPath;
        [GenerateDependencyProperty]
        private bool addressBarReadonly;
        [GenerateDependencyProperty]
        private string leftBrowserUrl;
        [GenerateDependencyProperty]
        private string rightBrowserUrl;
        [GenerateDependencyProperty]
        private bool browsersEnabled;
        [GenerateDependencyProperty]
        private bool autoPlaceOnScreen;
        [GenerateDependencyProperty]
        private string desiredScreen;
        [GenerateDependencyProperty]
        private string shortcutFolder;
        [GenerateDependencyProperty]
        private bool shortcutPlaced;
        [GenerateDependencyProperty]
        private bool autoCheckUpdate;

        public SettingsView(
            ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions,
            IViewManager viewManager)
        {
            this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull(nameof(liveUpdateableOptions));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.InitializeComponent();
            this.LoadSettings();
        }

        private void LoadSettings()
        {
            var config = this.liveUpdateableOptions.Value;
            this.AddressBarReadonly = config.AddressBarReadonly;
            this.ToolboxPath = config.ToolboxPath;
            this.LeftBrowserUrl = config.LeftBrowserDefault;
            this.RightBrowserUrl = config.RightBrowserDefault;
            this.ToolboxAutoLaunch = config.ToolboxAutoLaunch;
            this.TexmodPath = config.TexmodPath;
            this.BrowsersEnabled = config.BrowsersEnabled;
            this.AutoPlaceOnScreen = config.SetGuildwarsWindowSizeOnLaunch;
            this.DesiredScreen = config.DesiredGuildwarsScreen.ToString();
            this.ShortcutFolder = config.ShortcutLocation;
            this.ShortcutPlaced = config.PlaceShortcut;
            this.AutoCheckUpdate = config.AutoCheckUpdate;
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            var currentConfig = this.liveUpdateableOptions.Value;
            currentConfig.ToolboxPath = this.ToolboxPath;
            currentConfig.AddressBarReadonly = this.AddressBarReadonly;
            currentConfig.LeftBrowserDefault = this.LeftBrowserUrl;
            currentConfig.RightBrowserDefault = this.RightBrowserUrl;
            currentConfig.ToolboxAutoLaunch = this.ToolboxAutoLaunch;
            currentConfig.TexmodPath = this.TexmodPath;
            currentConfig.BrowsersEnabled = this.BrowsersEnabled;
            currentConfig.SetGuildwarsWindowSizeOnLaunch = this.AutoPlaceOnScreen;
            currentConfig.DesiredGuildwarsScreen = int.Parse(this.DesiredScreen);
            currentConfig.ShortcutLocation = this.ShortcutFolder;
            currentConfig.PlaceShortcut = this.ShortcutPlaced;
            currentConfig.AutoCheckUpdate = this.AutoCheckUpdate;
            this.liveUpdateableOptions.UpdateOption();
            this.viewManager.ShowView<SettingsCategoryView>();
        }

        private void ToolboxFilePickerGlyph_Clicked(object sender, EventArgs e)
        {
            var filePicker = new Microsoft.Win32.OpenFileDialog()
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

        private void TexmodFilePickerGlyph_Clicked(object sender, EventArgs e)
        {
            var filePicker = new Microsoft.Win32.OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "exe",
                Multiselect = false
            };
            if (filePicker.ShowDialog() is true)
            {
                this.TexmodPath = filePicker.FileName;
            }
        }

        private void ShortcutFolderPickerGlyph_Clicked(object sender, EventArgs e)
        {
            var folderPicker = new FolderBrowserDialog()
            {
                Description = "Select shortcut folder",
                UseDescriptionForTitle = true,
                SelectedPath = this.ShortcutFolder,
                ShowNewFolderButton = true
            };
            if (folderPicker.ShowDialog() is DialogResult.OK)
            {
                this.ShortcutFolder = folderPicker.SelectedPath;
            }
        }

        private void ScreenPickerGlyph_Clicked(object sender, EventArgs e)
        {
            this.viewManager.ShowView<ScreenChoiceView>();
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.viewManager.ShowView<SettingsCategoryView>();
        }

        private void TextBox_AllowOnlyNumbers(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (int.TryParse(e.Text, out _) is false)
            {
                e.Handled = true;
            }
        }
    }
}
