using Daybreak.Shared.Models.Themes;

namespace Daybreak.Shared.Services.Initialization;
public interface IThemeProducer
{
    void RegisterTheme(Theme theme);
}
