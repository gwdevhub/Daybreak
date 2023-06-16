using Daybreak.Models.Trade;
using Daybreak.Services.Navigation;
using Daybreak.Services.TradeChat;
using System;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Views.Trade;
/// <summary>
/// Interaction logic for TradeAlertSetupView.xaml
/// </summary>
public partial class TradeAlertSetupView : UserControl
{
    private readonly ITradeAlertingService tradeAlertingService;
    private readonly IViewManager viewManager;

    public TradeAlertSetupView(
        ITradeAlertingService tradeAlertingService,
        IViewManager viewManager)
    {
        this.tradeAlertingService = tradeAlertingService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<TradeAlertsView>();
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is not TradeAlert tradeAlert)
        {
            return;
        }

        this.tradeAlertingService.ModifyTradeAlert(tradeAlert);
    }
}
