using System;

namespace Daybreak.Services.Window;

public interface IWindowEventsHook<T> : IDisposable
    where T : System.Windows.Window
{
    void RegisterHookOnSizeOrMoveBegin(Action hook);
    void RegisterHookOnSizeOrMoveEnd(Action hook);
    void UnregisterHookOnSizeOrMoveBegin(Action hook);
    void UnregisterHookOnSizeOrMoveEnd(Action hook);
}
