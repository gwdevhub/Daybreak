using Daybreak.Configuration;
using Daybreak.Models;
using Daybreak.Services.Configuration;
using Daybreak.Services.KeyboardHook;
using Microsoft.Extensions.Logging;
using Pepa.Wpf.Utilities;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Extensions;
using System.Windows.Forms;

namespace Daybreak.Services.KeyboardMacros
{
    public sealed class MacroService : IMacroService
    {
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly IKeyboardHookService keyboardHookService;
        private readonly ILiveOptions<ApplicationConfiguration> liveOptions;
        private readonly HashSet<Keys> KeysDown = new();

        private bool gameActive;
        private IntPtr gwWindowHwnd;

        public MacroService(
            IKeyboardHookService keyboardHookService,
            ILiveOptions<ApplicationConfiguration> liveOptions)
        {
            this.keyboardHookService = keyboardHookService.ThrowIfNull(nameof(keyboardHookService));
            this.liveOptions = liveOptions.ThrowIfNull(nameof(liveOptions));

            this.keyboardHookService.KeyboardPressed += this.KeyboardHookService_KeyboardPressed;
            this.SetupGameActiveChecker();
        }

        private void SetupGameActiveChecker()
        {
            TaskExtensions.RunPeriodicAsync(() =>
            {
                var windowHandle = NativeMethods.GetForegroundWindow();
                var windowNameLength = NativeMethods.GetWindowTextLength(windowHandle);
                var sb = new StringBuilder(windowNameLength);
                _ = NativeMethods.GetWindowText(windowHandle, sb, windowNameLength + 1);
                if (sb.ToString() != "Guild Wars")
                {
                    this.gameActive = false;
                    return;
                }

                this.gwWindowHwnd = windowHandle;
                this.gameActive = true;
            },
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(33),
            cancellationTokenSource.Token);
        }

        private void KeyboardHookService_KeyboardPressed(object sender, KeyboardHookEventArgs e)
        {
            if (this.gameActive && this.liveOptions.Value.ExperimentalFeatures.CanInterceptKeys)
            {
                if (e.KeyboardState == KeyboardState.KeyDown)
                {
                    this.KeysDown.Add(e.KeyboardInput.Key);
                }
                else if (e.KeyboardState == KeyboardState.KeyUp)
                {
                    this.KeysDown.Remove(e.KeyboardInput.Key);
                    return;
                }

                e.Handled = this.liveOptions.Value.ExperimentalFeatures.Macros
                    .Where(keyMacro => MacroContainsKey(keyMacro, e.KeyboardInput.Key))
                    .Where(this.MacroHit)
                    .Do(this.HandleMacro)
                    .Any();
            }
        }

        public void OnClosing()
        {
            this.cancellationTokenSource.Cancel();
        }

        public void OnStartup()
        {
        }
        

        private bool MacroHit(KeyMacro keyMacro)
        {
            return this.KeysDown.Intersect(keyMacro.Keys).OrderBy(key => key).SequenceEqual(keyMacro.Keys.OrderBy(key => key));
        }

        private void HandleMacro(KeyMacro keyMacro)
        {
            // TODO: Propagate key to guildwars executable.
        }

        private static bool MacroContainsKey(KeyMacro keyMacro, Keys lastKey)
        {
            return keyMacro.Keys.Contains(lastKey);
        }
    }
}
