using Daybreak.Shared.Models.Trade;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.TradeChat;
using System;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Views.Trade;
/// <summary>
/// Interaction logic for TradeAlertsView.xaml
/// </summary>
public partial class TradeAlertsView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly ITradeAlertingService tradeAlertingService;

    public ObservableCollection<ITradeAlert> TradeAlerts { get; set; } = [];

    public TradeAlertsView(
        IViewManager viewManager,
        ITradeAlertingService tradeAlertingService)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.tradeAlertingService = tradeAlertingService.ThrowIfNull();
        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.TradeAlerts.ClearAnd().AddRange(this.tradeAlertingService.TradeAlerts);
    }

    private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement frameworkElement ||
            frameworkElement.DataContext is not ITradeAlert tradeAlert)
        {
            return;
        }

        this.tradeAlertingService.ModifyTradeAlert(tradeAlert);
    }

    private void AddButton_Clicked(object _, EventArgs __)
    {
        this.viewManager.ShowView<TradeAlertsChoiceView>();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        if(sender is not FrameworkElement frameworkElement ||
           frameworkElement.DataContext is not ITradeAlert alert)
        {
            return;
        }

        if (alert is TradeAlert)
        {
            this.viewManager.ShowView<TradeAlertSetupView>(alert);
        }
        else if (alert is QuoteAlert)
        {
            this.viewManager.ShowView<QuoteAlertSetupView>(alert);
        }
    }

    private void BinButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not FrameworkElement frameworkElement ||
           frameworkElement.DataContext is not ITradeAlert alert)
        {
            return;
        }

        this.tradeAlertingService.DeleteTradeAlert(alert.Id);
        this.TradeAlerts.Remove(alert);
    }
}
