using TrailBlazr.ViewModels;

namespace Daybreak.Views;

public sealed class GuildWarsMarketViewModel : ViewModelBase<GuildWarsMarketViewModel, GuildWarsMarketView>
{
    public string GwMarketUrl { get; } = "https://gwmarket.net/";
}
