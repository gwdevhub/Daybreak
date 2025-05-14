using System.Windows.Extensions.Services;

namespace Daybreak.Shared.Services.Shortcuts;

public interface IShortcutManager : IApplicationLifetimeService
{
    bool ShortcutEnabled { get; set; }
}
