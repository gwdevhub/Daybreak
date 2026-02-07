using Daybreak.Shared.Utils;

namespace Daybreak.Shared.Services.Window;

/// <summary>
/// Provides platform-specific window manipulation operations such as
/// drag-move and resize via native window manager APIs.
/// </summary>
public interface IWindowManipulationService
{
    /// <summary>
    /// Initiates a window drag-move operation.
    /// </summary>
    void DragWindow();

    /// <summary>
    /// Initiates a window resize operation in the specified direction.
    /// </summary>
    void ResizeWindow(ResizeDirection direction);
}
