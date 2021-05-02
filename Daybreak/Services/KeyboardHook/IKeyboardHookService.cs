using Daybreak.Models;
using Daybreak.Services.ApplicationLifetime;
using System;

namespace Daybreak.Services.KeyboardHook
{
    public interface IKeyboardHookService : IApplicationLifetimeService
    {
        event EventHandler<KeyboardHookEventArgs> KeyboardPressed;
    }
}
