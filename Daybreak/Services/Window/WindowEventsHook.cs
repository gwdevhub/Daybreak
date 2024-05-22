using Daybreak.Behaviors;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Interop;

namespace Daybreak.Services.Window;

internal sealed class WindowEventsHook<T> : IWindowEventsHook<T>
    where T : System.Windows.Window
{
    private const int WM_ENTERSIZEMOVE = 0x0231;
    private const int WM_EXITSIZEMOVE = 0x0232;

    private readonly List<Action> sizeMoveBeginHooks = [];
    private readonly List<Action> sizeMoveEndHooks = [];
    private readonly T window;

    private HwndSource? hwndSource;

    public WindowEventsHook(
        T window)
    {
        this.window = window.ThrowIfNull();

        // If the window is loaded, hook directly. Otherwise, wait for the window to finish loading before hooking
        if (this.window.IsLoaded)
        {
            this.HookIntoWindow();
        }
        else
        {
            this.window.Loaded += this.MainWindow_Loaded;
        }
    }

    public void RegisterHookOnSizeOrMoveBegin(Action hook)
    {
        this.sizeMoveBeginHooks.Add(hook);
    }

    public void RegisterHookOnSizeOrMoveEnd(Action hook)
    {
        this.sizeMoveEndHooks.Add(hook);
    }

    public void UnregisterHookOnSizeOrMoveBegin(Action hook)
    {
        this.sizeMoveBeginHooks.Remove(hook);
    }

    public void UnregisterHookOnSizeOrMoveEnd(Action hook)
    {
        this.sizeMoveEndHooks.Remove(hook);
    }

    private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.HookIntoWindow();
    }

    private void HookIntoWindow()
    {
        this.hwndSource = PresentationSource.FromVisual(this.window).Cast<HwndSource>();
        this.hwndSource.AddHook(this.WndProc);
        var contentCache = this.window.Content.Cast<UIElement>();
        var decorator = new LayoutFreezeDecorator();
        this.window.Content = decorator;
        decorator.Child = contentCache;
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case WM_ENTERSIZEMOVE:
                // Disable layout changes while resizing
                if (this.window.Content is LayoutFreezeDecorator freezeDecorator)
                {
                    freezeDecorator.IsLayoutFrozen = true;
                }

                this.window.Dispatcher.Invoke(() =>
                {
                    foreach (var action in this.sizeMoveBeginHooks)
                    {
                        action();
                    }
                });

                break;
            case WM_EXITSIZEMOVE:
                // Re-enable layout changes after resizing is done
                if (this.window.Content is LayoutFreezeDecorator freezeDecorator2)
                {
                    freezeDecorator2.IsLayoutFrozen = false;
                    // Force layout recalculation to fit the new content to the new size
                    freezeDecorator2.InvalidateArrange();
                    freezeDecorator2.InvalidateMeasure();
                    freezeDecorator2.InvalidateVisual();
                }

                this.window.Dispatcher.Invoke(() =>
                {
                    foreach (var action in this.sizeMoveEndHooks)
                    {
                        action();
                    }
                });

                break;
        }

        return IntPtr.Zero;
    }

    public void Dispose()
    {
        this.hwndSource?.Dispose();
        this.sizeMoveBeginHooks.Clear();
        this.sizeMoveEndHooks.Clear();
        this.window.Loaded -= this.MainWindow_Loaded;
    }
}
