using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.Keyboard;

public interface IKeyboardHookService
{
    event EventHandler<KeyboardEventArgs>? KeyDown;
    event EventHandler<KeyboardEventArgs>? KeyUp;

    void Start();
    void Stop();
}
