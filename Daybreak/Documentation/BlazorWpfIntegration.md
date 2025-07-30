# Blazor WebView with Window Dragging Setup

This implementation demonstrates how to properly set up a Blazor WebView in a WPF application with custom window dragging functionality that works even when the WebView intercepts mouse events.

## The Problem

When using a Blazor WebView in WPF, the WebView intercepts mouse events, so WPF's built-in `DragMove()` method fails with the error:
> "Can only call DragMove when primary mouse button is down"

This happens because WPF doesn't see the mouse button as being pressed down since the WebView captures the events first.

## The Solution

We use Win32 API calls to simulate window dragging behavior that bypasses the need for WPF to track mouse button state.

## Architecture

### 1. Service Interface (`IWindowInteropService`)
- Defines the contract for communication between Blazor and WPF
- Provides an event `WindowDragRequested` that WPF can subscribe to
- Provides methods `RequestWindowDrag()` and `RequestWindowDragWithHandle(IntPtr)`

### 2. Service Implementation (`WindowInteropService`)
- Uses Win32 API calls (`SendMessage` and `ReleaseCapture`) for reliable window dragging
- `RequestWindowDragWithHandle()` sends `WM_NCLBUTTONDOWN` message with `HTCAPTION` to simulate title bar dragging
- Registered as a singleton in the DI container

### 3. WPF Integration (`MainWindow`)
- Subscribes to the `WindowDragRequested` event in `SetupWindowInteropService()`
- Gets the window handle using `WindowInteropHelper` and calls the Win32 API method
- BlazorWebView is positioned to cover the entire window

### 4. Blazor Component (`Index.razor`)
- Injects the `IWindowInteropService` using `@inject`
- Has a custom title bar area that calls `WindowInterop.RequestWindowDrag()` on mouse down
- Works reliably regardless of WebView mouse event interception

## Key Benefits

1. **Reliable Window Dragging**: Works even when WebView intercepts mouse events
2. **Proper Separation of Concerns**: Blazor handles UI logic, WPF handles window operations
3. **Type Safety**: Uses proper C# interfaces instead of string-based messaging
4. **Dependency Injection**: Leverages the existing DI container
5. **Cross-Platform Compatibility**: Win32 API calls work on all Windows versions

## Technical Implementation

### Win32 API Constants
```csharp
private const int WM_NCLBUTTONDOWN = 0x00A1;  // Non-client left button down
private const int HTCAPTION = 2;              // Title bar area
```

### Window Dragging Process
1. Blazor component detects mouse down on title bar
2. Calls `WindowInterop.RequestWindowDrag()`
3. Service raises `WindowDragRequested` event
4. WPF gets window handle and calls `RequestWindowDragWithHandle()`
5. Win32 API simulates title bar drag operation

## Usage in Other Components

To use window dragging in other Blazor components:

```razor
@using Daybreak.Services.WindowInterop
@inject IWindowInteropService WindowInterop

<div @onmousedown="() => WindowInterop.RequestWindowDrag()">
    Drag me to move the window
</div>
```

## Extending the Service

You can easily extend `IWindowInteropService` to support other window operations:

```csharp
public interface IWindowInteropService
{
    event EventHandler? WindowDragRequested;
    event EventHandler? WindowMinimizeRequested;
    event EventHandler? WindowMaximizeRequested;
    
    void RequestWindowDrag();
    void RequestWindowDragWithHandle(IntPtr windowHandle);
    void RequestWindowMinimize();
    void RequestWindowMaximize();
}
```

This approach solves the mouse event interception problem and provides a robust foundation for window management in Blazor/WPF hybrid applications.