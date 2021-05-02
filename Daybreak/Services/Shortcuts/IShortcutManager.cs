using Daybreak.Services.ApplicationLifetime;

namespace Daybreak.Services.Shortcuts
{
    public interface IShortcutManager : IApplicationLifetimeService
    {
        bool ShortcutEnabled { get; set; }
    }
}
