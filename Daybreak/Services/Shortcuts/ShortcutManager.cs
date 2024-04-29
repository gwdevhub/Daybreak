using Daybreak.Configuration.Options;
using Daybreak.Services.Options;
using ShellLink;
using System.Configuration;
using System.Diagnostics;
using System.Extensions;
using System.IO;

namespace Daybreak.Services.Shortcuts;

//TODO: Fix dependency on IConfigurationManager
internal sealed class ShortcutManager : IShortcutManager
{
    private const string ShortcutName = "Daybreak.lnk";

    private readonly ILiveOptions<LauncherOptions> liveOptions;

    public bool ShortcutEnabled {
        get => this.ShortcutExists();
        set
        {
            if (value is true)
            {
                this.CreateShortcut();
            }
            else
            {
                this.RemoveShortcut();
            }
        }
    }

    public ShortcutManager(
        IOptionsUpdateHook optionsUpdateHook,
        ILiveOptions<LauncherOptions> liveOptions)
    {
        this.liveOptions = liveOptions.ThrowIfNull(nameof(liveOptions));
        optionsUpdateHook.RegisterHook<LauncherOptions>(this.LoadConfiguration);
        this.LoadConfiguration();
    }

    private void LoadConfiguration()
    {
        var shortcutEnabled = this.liveOptions.Value.PlaceShortcut;
        if (shortcutEnabled && this.ShortcutEnabled is false)
        {
            this.ShortcutEnabled = true;
        }
        else if (shortcutEnabled is false && this.ShortcutEnabled is true)
        {
            this.ShortcutEnabled = false;
        }
    }

    private bool ShortcutExists()
    {
        var shortcutFolder = this.liveOptions.Value.ShortcutLocation;
        var shortcutPath = $"{shortcutFolder}\\{ShortcutName}";
        if (File.Exists(shortcutPath))
        {
            var shortcut = Shortcut.ReadFromFile(shortcutPath);
            var currentExecutable = Process.GetCurrentProcess()?.MainModule?.FileName;
            if (shortcut.LinkTargetIDList?.Path?.Equals(currentExecutable) is true)
            {
                return true;
            }

            return false;
        }

        return false;
    }

    private void CreateShortcut()
    {
        if (this.ShortcutExists())
        {
            return;
        }

        var shortcutFolder = this.liveOptions.Value.ShortcutLocation;
        var shortcutPath = $"{shortcutFolder}\\{ShortcutName}";
        var currentExecutable = Process.GetCurrentProcess()?.MainModule?.FileName;
        var shortcut = Shortcut.CreateShortcut(currentExecutable);
        shortcut.StringData = new ShellLink.Structures.StringData
        {
            WorkingDir = Path.GetDirectoryName(currentExecutable),
            RelativePath = "Daybreak.exe"
        };

        try
        {
            shortcut.WriteToFile(shortcutPath);
        }
        catch
        {
        }
    }

    private void RemoveShortcut()
    {
        if (this.ShortcutExists() is false)
        {
            return;
        }

        var shortcutFolder = this.liveOptions.Value.ShortcutLocation;
        var shortcutPath = $"{shortcutFolder}\\{ShortcutName}";
        File.Delete(shortcutPath);
    }

    public void OnStartup()
    {
    }

    public void OnClosing()
    {
    }
}
