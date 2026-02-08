using Daybreak.Shared.Models.Trade;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.TradeChat;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Trade;
public sealed class TradeQuoteViewModel(
    INotificationService notificationService,
    IViewManager viewManager,
    ITradeAlertingService tradeAlertingService)
    : ViewModelBase<TradeQuoteViewModel, TradeQuoteView>
{
    private readonly INotificationService notificationService = notificationService;
    private readonly IViewManager viewManager = viewManager;
    private readonly ITradeAlertingService tradeAlertingService = tradeAlertingService;

    public QuoteAlert? Alert { get; private set; }

    public override ValueTask Initialize(CancellationToken cancellationToken)
    {
        if (this.tradeAlertingService.TradeAlerts.FirstOrDefault(t => t.Id == this.View?.AlertId) is not QuoteAlert alert)
        {
            this.notificationService.NotifyError(
                "Alert not found",
                $"The specified alert was not found by id {this.View?.AlertId}");
            this.viewManager.ShowView<TradeAlertsView>();
            return base.Initialize(cancellationToken);
        }

        this.Alert = alert;
        this.RefreshView();
        return base.Initialize(cancellationToken);
    }

    public void SaveAlertChanges()
    {
        if (this.Alert is null)
        {
            return;
        }

        this.tradeAlertingService.ModifyTradeAlert(this.Alert);
    }

    public void DeleteAlert()
    {
        if (this.Alert is null)
        {
            return;
        }

        this.tradeAlertingService.DeleteTradeAlert(this.Alert.Id);
        this.viewManager.ShowView<TradeAlertsView>();
    }
}
