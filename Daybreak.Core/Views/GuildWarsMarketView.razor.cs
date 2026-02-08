using TrailBlazr.ViewModels;

namespace Daybreak.Views;

public sealed class GuildWarsMarketViewModel : ViewModelBase<GuildWarsMarketViewModel, GuildWarsMarketView>
{
    public string GwMarketUrl { get; } = "https://v2.gwmarket.net/";
}
