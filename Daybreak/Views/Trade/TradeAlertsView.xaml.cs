using Daybreak.Configuration.Options;
using Daybreak.Models.Trade;
using Daybreak.Services.Navigation;
using Daybreak.Services.TradeChat;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
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

    public ObservableCollection<TradeAlert> TradeAlerts { get; set; } = [];

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
            frameworkElement.DataContext is not TradeAlert tradeAlert)
        {
            return;
        }

        this.tradeAlertingService.ModifyTradeAlert(tradeAlert);
    }

    private void AddButton_Clicked(object _, EventArgs __)
    {
        var tradeAlert = new TradeAlert
        {
            Enabled = false,
            Name = "New trade alert"
        };

        this.tradeAlertingService.AddTradeAlert(tradeAlert);
        this.TradeAlerts.Add(tradeAlert);
        this.viewManager.ShowView<TradeAlertSetupView>(tradeAlert);
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        if(sender is not FrameworkElement frameworkElement ||
           frameworkElement.DataContext is not TradeAlert alert)
        {
            return;
        }

        this.viewManager.ShowView<TradeAlertSetupView>(alert);
    }

    private void BinButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not FrameworkElement frameworkElement ||
           frameworkElement.DataContext is not TradeAlert alert)
        {
            return;
        }

        this.tradeAlertingService.DeleteTradeAlert(alert.Id);
        this.TradeAlerts.Remove(alert);
    }
}
