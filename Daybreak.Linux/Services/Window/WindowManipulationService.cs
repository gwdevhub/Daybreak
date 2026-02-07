using Daybreak.Shared.Services.Window;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.Window;

//TODO: Implement window manipulation for Linux. This is currently a stub that does nothing, as default window functionality is kept on Linux
internal sealed class WindowManipulationService : IWindowManipulationService
{
    private readonly ILogger<WindowManipulationService> logger;

    public WindowManipulationService(ILogger<WindowManipulationService> logger)
    {
        this.logger = logger;
    }

    public void DragWindow(nint windowHandle)
    {
    }

    public void ResizeWindow(nint windowHandle, ResizeDirection direction)
    {
    }
}
