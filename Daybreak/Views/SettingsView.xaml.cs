using Daybreak.Configuration.Options;
using Daybreak.Services.Navigation;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Extensions;
using System.Windows.Forms;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for SettingsView.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
public partial class SettingsView : System.Windows.Controls.UserControl
{
    private readonly ILiveUpdateableOptions<BrowserOptions> browserOptions;
    private readonly ILiveUpdateableOptions<LauncherOptions> launcherOptions;
    private readonly ILiveUpdateableOptions<UModOptions> uModOptions;
    private readonly ILiveUpdateableOptions<ToolboxOptions> toolboxOptions;
    private readonly IViewManager viewManager;

    [GenerateDependencyProperty]
    private bool uModEnabled;
    [GenerateDependencyProperty]
    private string uModPath = string.Empty;
    [GenerateDependencyProperty]
    private bool toolboxEnabled;
    [GenerateDependencyProperty]
    private string toolboxPath = string.Empty;
    [GenerateDependencyProperty]
    private bool addressBarReadonly;
    [GenerateDependencyProperty]
    private bool browsersEnabled;
    [GenerateDependencyProperty]
    private bool autoPlaceOnScreen;
    [GenerateDependencyProperty]
    private string desiredScreen = string.Empty;
    [GenerateDependencyProperty]
    private string shortcutFolder = string.Empty;
    [GenerateDependencyProperty]
    private bool shortcutPlaced;
    [GenerateDependencyProperty]
    private bool autoCheckUpdate;

    public SettingsView(
        ILiveUpdateableOptions<UModOptions> uModOptions,
        ILiveUpdateableOptions<ToolboxOptions> toolboxOptions,
        ILiveUpdateableOptions<BrowserOptions> browserOptions,
        ILiveUpdateableOptions<LauncherOptions> launcherOptions,
        IViewManager viewManager)
    {
        this.uModOptions = uModOptions.ThrowIfNull();
        this.toolboxOptions = toolboxOptions.ThrowIfNull();
        this.browserOptions = browserOptions.ThrowIfNull();
        this.launcherOptions = launcherOptions.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
        this.LoadSettings();
    }

    private void LoadSettings()
    {
        this.AddressBarReadonly = this.browserOptions.Value.AddressBarReadonly;
        this.BrowsersEnabled = this.browserOptions.Value.Enabled;
        this.ToolboxPath = this.toolboxOptions.Value.Path;
        this.ToolboxEnabled = this.toolboxOptions.Value.Enabled;
        this.UModPath = this.uModOptions.Value.Path;
        this.UModEnabled = this.uModOptions.Value.Enabled;
        this.AutoPlaceOnScreen = this.launcherOptions.Value.SetGuildwarsWindowSizeOnLaunch;
        this.DesiredScreen = this.launcherOptions.Value.DesiredGuildwarsScreen.ToString();
        this.ShortcutFolder = this.launcherOptions.Value.ShortcutLocation;
        this.ShortcutPlaced = this.launcherOptions.Value.PlaceShortcut;
        this.AutoCheckUpdate = this.launcherOptions.Value.AutoCheckUpdate;
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        var browserOptions = this.browserOptions.Value;
        var toolboxOptions = this.toolboxOptions.Value;
        var umodOptions = this.uModOptions.Value;
        var launcherOptions = this.launcherOptions.Value;
        browserOptions.AddressBarReadonly = this.AddressBarReadonly;
        browserOptions.Enabled = this.BrowsersEnabled;
        toolboxOptions.Path = this.ToolboxPath;
        toolboxOptions.Enabled = this.ToolboxEnabled;
        umodOptions.Path = this.UModPath;
        umodOptions.Enabled = this.UModEnabled;
        launcherOptions.SetGuildwarsWindowSizeOnLaunch = this.AutoPlaceOnScreen;
        if (int.TryParse(this.DesiredScreen, out var desiredScreen) is false)
        {
            throw new InvalidOperationException("Invalid desired screen value");
        }

        launcherOptions.DesiredGuildwarsScreen = desiredScreen;
        launcherOptions.ShortcutLocation = this.ShortcutFolder;
        launcherOptions.PlaceShortcut = this.ShortcutPlaced;
        launcherOptions.AutoCheckUpdate = this.AutoCheckUpdate;

        this.browserOptions.UpdateOption();
        this.toolboxOptions.UpdateOption();
        this.uModOptions.UpdateOption();
        this.launcherOptions.UpdateOption();
        this.viewManager.ShowView<LauncherView>();
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

    private void TextBox_AllowOnlyNumbers(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (int.TryParse(e.Text, out _) is false)
        {
            e.Handled = true;
        }
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

    private void UModFilePickerGlyph_Clicked(object sender, EventArgs e)
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
            this.UModPath = filePicker.FileName;
        }
    }
}
