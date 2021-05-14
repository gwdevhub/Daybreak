using Daybreak.Models;
using Microsoft.Extensions.Logging;
using Pepa.Wpf.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Extensions;
using System.Linq;

namespace Daybreak.Services.Screens
{
    public sealed class ScreenManager : IScreenManager
    {
        private readonly ILogger<ScreenManager> logger;

        public IEnumerable<Screen> Screens { get; } = WpfScreenHelper.Screen.AllScreens
            .Select((screen, index) => new Screen { Id = index, Size = screen.Bounds });

        public ScreenManager(
            ILogger<ScreenManager> logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
        }

        public void MoveGuildwarsToScreen(Screen screen)
        {
            this.logger.LogInformation($"Attempting to move guildwars to screen {screen.Id}");
            var hwnd = GetMainWindowHandle();
            NativeMethods.SetWindowPos(hwnd, NativeMethods.HWND_TOP, screen.Size.Left.ToInt(), screen.Size.Top.ToInt(), screen.Size.Width.ToInt(), screen.Size.Height.ToInt(), NativeMethods.SWP_SHOWWINDOW);
        }

        private static IntPtr GetMainWindowHandle()
        {
            var process = Process.GetProcessesByName("gw").FirstOrDefault();
            return process is not null ? process.MainWindowHandle : throw new InvalidOperationException("Could not find guildwars process");
        }
    }
}
