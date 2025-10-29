using Daybreak.Shared.Services.Screens;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Mods;
public sealed class ScreenSelectorViewModel(IGuildwarsScreenPlacer guildwarsScreenPlacer)
    : ViewModelBase<ScreenSelectorViewModel, ScreenSelectorView>
{
    private readonly IGuildwarsScreenPlacer guildwarsScreenPlacer = guildwarsScreenPlacer;
}
