using System.Windows.Extensions.Services;

namespace Daybreak.Services.Shortcuts
{
    public interface IShortcutManager : IApplicationLifetimeService
    {
        bool ShortcutEnabled { get; set; }
    }
}
