using Daybreak.Models.Trade;
using Daybreak.Services.Navigation;
using Daybreak.Services.TradeChat;
using System;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Trade;
/// <summary>
/// Interaction logic for TradeAlertsChoiceView.xaml
/// </summary>
public partial class TradeAlertsChoiceView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly ITradeAlertingService tradeAlertingService;

    public TradeAlertsChoiceView(
        IViewManager viewManager,
        ITradeAlertingService tradeAlertingService)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.tradeAlertingService = tradeAlertingService.ThrowIfNull();
        this.InitializeComponent();
    }

    private void AddTradeAlertButton_Clicked(object _, EventArgs __)
    {
        var tradeAlert = new TradeAlert
        {
            Enabled = false,
            Name = "New trade alert"
        };

        this.tradeAlertingService.AddTradeAlert(tradeAlert);
        this.viewManager.ShowView<TradeAlertSetupView>(tradeAlert);
    }

    private void AddBuyQuoteAlertButton_Clicked(object _, EventArgs __)
    {
        var tradeAlert = new QuoteAlert
        {
            Enabled = false,
            Name = "New buy quote alert",
            TraderQuoteType = Services.TradeChat.Models.TraderQuoteType.Buy
        };

        this.tradeAlertingService.AddTradeAlert(tradeAlert);
        this.viewManager.ShowView<QuoteAlertSetupView>(tradeAlert);
    }

    private void AddSellQuoteAlertButton_Clicked(object _, EventArgs __)
    {
        var tradeAlert = new QuoteAlert
        {
            Enabled = false,
            Name = "New sell quote alert",
            TraderQuoteType = Services.TradeChat.Models.TraderQuoteType.Sell
        };

        this.tradeAlertingService.AddTradeAlert(tradeAlert);
        this.viewManager.ShowView<QuoteAlertSetupView>(tradeAlert);
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<TradeAlertsView>();
    }
}
