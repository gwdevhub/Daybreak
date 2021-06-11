using Daybreak.Models;
using System;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.KeyboardHook
{
    public interface IKeyboardHookService : IApplicationLifetimeService
    {
        event EventHandler<KeyboardHookEventArgs> KeyboardPressed;
    }
}
