using Daybreak.Services.TradeChat.Models;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Trade;
public class TradeChatViewModel
    : ViewModelBase<TradeChatViewModel, TradeChatView>
{
    public string? TradeChatUrl { get; private set; }

    public override ValueTask ParametersSet(TradeChatView view, CancellationToken cancellationToken)
    {
        this.TradeChatUrl = view.Source switch
        {
            nameof(TraderSource.Ascalon) => "https://ascalon.gwtoolbox.com/",
            nameof(TraderSource.Kamadan) => "https://kamadan.gwtoolbox.com/",
            _ => throw new NotSupportedException($"The source {view.Source} is not supported."),
        };

        return base.ParametersSet(view, cancellationToken);
    }
}
