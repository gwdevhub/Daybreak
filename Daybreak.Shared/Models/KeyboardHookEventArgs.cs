namespace Daybreak.Models;

public sealed class KeyboardHookEventArgs
{
    public bool Handled { get; set; }
    public KeyboardState KeyboardState { get; }
    public KeyboardInput KeyboardInput { get; }

    public KeyboardHookEventArgs(
        KeyboardState keyboardState,
        KeyboardInput keyboardInput)
    {
        this.KeyboardState = keyboardState;
        this.KeyboardInput = keyboardInput;
    }
}
