﻿using Daybreak.Models.Guildwars;
using Daybreak.Models.Trade;
using Daybreak.Services.Navigation;
using Daybreak.Services.TradeChat;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Views.Trade;
/// <summary>
/// Interaction logic for QuoteAlertSetupView.xaml
/// </summary>
public partial class QuoteAlertSetupView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly ITradeAlertingService tradeAlertingService;

    public List<ItemBase> AvailableItems { get; } = ItemBase.AllItems.Where(i => i.Modifiers is null).ToList();

    public QuoteAlertSetupView(
        IViewManager viewManager,
        ITradeAlertingService tradeAlertingService)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.tradeAlertingService = tradeAlertingService.ThrowIfNull();
        this.InitializeComponent();
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<TradeAlertsView>();
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is not QuoteAlert tradeAlert)
        {
            return;
        }

        this.tradeAlertingService.ModifyTradeAlert(tradeAlert);
    }

    private void TextBlock_AllowOnlyNumeric(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        foreach (char c in e.Text)
        {
            if (!char.IsDigit(c))
            {
                e.Handled = true;
            }
        }
    }
}
