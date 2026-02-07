using Daybreak.Shared.Utils;

namespace Daybreak.Shared.Services.Window;

/// <summary>
/// Provides platform-specific window manipulation operations such as
/// drag-move and resize via native window manager APIs.
/// </summary>
public interface IWindowManipulationService
{
    /// <summary>
    /// Initiates a window drag-move operation on the given window handle.
    /// </summary>
    void DragWindow(nint windowHandle);

    /// <summary>
    /// Initiates a window resize operation on the given window handle
    /// in the specified direction.
    /// </summary>
    void ResizeWindow(nint windowHandle, ResizeDirection direction);
}
