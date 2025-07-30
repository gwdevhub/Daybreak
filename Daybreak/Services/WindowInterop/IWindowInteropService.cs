using System;

namespace Daybreak.Services.WindowInterop
{
    public interface IWindowInteropService
    {
        event EventHandler? WindowDragRequested;
        void RequestWindowDrag();
        void RequestWindowDragWithHandle(IntPtr windowHandle);
    }
}
