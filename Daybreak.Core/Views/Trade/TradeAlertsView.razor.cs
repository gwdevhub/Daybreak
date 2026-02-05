using Daybreak.Models;
using Daybreak.Shared.Models.Trade;
using Daybreak.Shared.Services.TradeChat;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Trade;
public sealed class TradeAlertsViewModel(
    IViewManager viewManager,
    ITradeAlertingService tradeAlertingService)
    : ViewModelBase<TradeAlertsViewModel, TradeAlertsView>
{
    private readonly IViewManager viewManager = viewManager;
    private readonly ITradeAlertingService tradeAlertingService = tradeAlertingService;

    public IEnumerable<TradeAlertViewModel> TradeAlerts => this.tradeAlertingService.TradeAlerts.Select(ConvertToViewModel);

    public void CreateMessageAlert()
    {
        var messageAlert = new TradeAlert
        {
            Name = "New Message Alert",
        };

        this.tradeAlertingService.AddTradeAlert(messageAlert);
        this.RefreshView();
    }

    public void CreateQuoteAlert()
    {
        var quoteAlert = new QuoteAlert
        {
            Name = "New Quote Alert",
        };

        this.tradeAlertingService.AddTradeAlert(quoteAlert);
        this.RefreshView();
    }

    public void DeleteAlert(TradeAlertViewModel alert)
    {
        this.tradeAlertingService.DeleteTradeAlert(alert.TradeAlert.Id);
        this.RefreshView();
    }

    public void SelectAlert(TradeAlertViewModel alert)
    {
        if (alert.TradeAlert is TradeAlert messageAlert)
        {
            this.viewManager.ShowView<TradeMessageView>((nameof(TradeMessageView.AlertId), messageAlert.Id));
        }
        else if (alert.TradeAlert is QuoteAlert quoteAlert)
        {
            this.viewManager.ShowView<TradeQuoteView>((nameof(TradeQuoteView.AlertId), quoteAlert.Id));
        }
    }

    public void ToggleAlert(TradeAlertViewModel alert)
    {
        alert.TradeAlert.Enabled = !alert.TradeAlert.Enabled;
        this.tradeAlertingService.ModifyTradeAlert(alert.TradeAlert);
        this.RefreshView();
    }

    private static TradeAlertViewModel ConvertToViewModel(ITradeAlert alert)
    {
        return new TradeAlertViewModel
        {
            TradeAlert = alert,
            Type = alert switch
            {
                TradeAlert => TradeAlertViewModel.TradeAlertType.Message,
                QuoteAlert => TradeAlertViewModel.TradeAlertType.Quote,
                _ => TradeAlertViewModel.TradeAlertType.Unknown
            }
        };
    }
}
