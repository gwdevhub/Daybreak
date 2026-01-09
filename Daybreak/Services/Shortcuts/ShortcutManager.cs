using Daybreak.Configuration.Options;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Shortcuts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ShellLink;
using System.Diagnostics;
using System.Extensions;

namespace Daybreak.Services.Shortcuts;

internal sealed class ShortcutManager : IShortcutManager, IHostedService
{
    private const string ShortcutName = "Daybreak.lnk";

    private readonly IOptionsMonitor<LauncherOptions> liveOptions;

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
        IOptionsMonitor<LauncherOptions> liveOptions)
    {
        this.liveOptions = liveOptions.ThrowIfNull(nameof(liveOptions));
        optionsUpdateHook.RegisterHook<LauncherOptions>(this.LoadConfiguration);
        this.LoadConfiguration();
    }

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void LoadConfiguration()
    {
        var shortcutEnabled = this.liveOptions.CurrentValue.PlaceShortcut;
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
        var shortcutFolder = this.liveOptions.CurrentValue.ShortcutLocation;
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

        var shortcutFolder = this.liveOptions.CurrentValue.ShortcutLocation;
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

        var shortcutFolder = this.liveOptions.CurrentValue.ShortcutLocation;
        var shortcutPath = $"{shortcutFolder}\\{ShortcutName}";
        File.Delete(shortcutPath);
    }
}
