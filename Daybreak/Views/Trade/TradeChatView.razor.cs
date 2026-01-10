using Daybreak.Services.TradeChat.Models;
using Daybreak.Shared.Services.Themes;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Trade;
public class TradeChatViewModel(
    IThemeManager themeManager)
    : ViewModelBase<TradeChatViewModel, TradeChatView>
{
    private readonly IThemeManager themeManager = themeManager;

    public string? TradeChatUrl => this.TraderSource switch
    {
        TraderSource.Ascalon => $"https://ascalon.gwtoolbox.com/?theme={(this.themeManager.IsLightMode ? "light" : "dark")}",
        TraderSource.Kamadan => $"https://kamadan.gwtoolbox.com/?theme={(this.themeManager.IsLightMode ? "light" : "dark")}",
        _ => null,
    };
    public TraderSource TraderSource { get; private set; }

    public override ValueTask Initialize(CancellationToken cancellationToken)
    {
        this.themeManager.ThemeChanged += this.ThemeManager_ThemeChanged;
        return base.Initialize(cancellationToken);
    }

    private void ThemeManager_ThemeChanged(object? sender, Shared.Models.Themes.Theme e)
    {
        this.RefreshView();
    }

    public override ValueTask ParametersSet(TradeChatView view, CancellationToken cancellationToken)
    {
        this.TraderSource = view.Source switch
        {
            nameof(TraderSource.Ascalon) => TraderSource.Ascalon,
            nameof(TraderSource.Kamadan) => TraderSource.Kamadan,
            _ => throw new NotSupportedException($"The source {view.Source} is not supported."),
        };

        return base.ParametersSet(view, cancellationToken);
    }
}
