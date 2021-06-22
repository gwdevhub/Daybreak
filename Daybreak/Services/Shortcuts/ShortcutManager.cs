using Daybreak.Configuration;
using Daybreak.Services.Configuration;
using ShellLink;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Windows.Extensions;

namespace Daybreak.Services.Shortcuts
{
    //TODO: Fix dependency on IConfigurationManager
    public sealed class ShortcutManager : IShortcutManager
    {
        private const string ShortcutName = "Daybreak.lnk";

        private readonly IConfigurationManager configurationManager;
        private readonly ILiveOptions<ApplicationConfiguration> liveOptions;

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
            IConfigurationManager configurationManager,
            ILiveOptions<ApplicationConfiguration> liveOptions)
        {
            this.configurationManager = configurationManager.ThrowIfNull(nameof(configurationManager));
            this.liveOptions = liveOptions.ThrowIfNull(nameof(liveOptions));
            this.configurationManager.ConfigurationChanged += (_, _) => this.LoadConfiguration();
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
                if (shortcut.ExtraData?.EnvironmentVariableDataBlock?.TargetAnsi?.Equals(currentExecutable) is true)
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
            shortcut.WriteToFile(shortcutPath);
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
}
