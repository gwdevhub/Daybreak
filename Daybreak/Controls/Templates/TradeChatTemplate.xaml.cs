using Daybreak.Models.Trade;
using Daybreak.Services.TradeChat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Threading;

namespace Daybreak.Controls.Templates;

/// <summary>
/// Interaction logic for TradeChatTemplate.xaml
/// </summary>
public partial class TradeChatTemplate : UserControl
{
    private readonly TimeSpan timeUpdateInterval = TimeSpan.FromSeconds(1);
    private readonly object collectionLock = new();
    private readonly DispatcherTimer dispatcherTimer = new();
    private CancellationTokenSource? cancellationTokenSource;
    private DateTime? lastQueryTime;
    private DateTime nextQueryTime = DateTime.Now;

    [GenerateDependencyProperty]
    private ITradeChatService tradeChatService = default!;
    [GenerateDependencyProperty]
    private string searchQuery = default!;
    [GenerateDependencyProperty]
    private bool searchByUsername = default;
    [GenerateDependencyProperty]
    private bool messageOverlayVisible;
    [GenerateDependencyProperty]
    private TraderMessage selectedTraderMessage = default!;

    public ObservableCollection<TraderMessageViewWrapper> TraderMessages { get; } = new();

    public TradeChatTemplate()
    {
        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == TradeChatServiceProperty &&
            this.TradeChatService is not null)
        {
            this.InitializeLatestTrades();
        }
    }

    private void TradeMessageTemplate_TraderMessageClicked(object _, TraderMessage traderMessage)
    {
        this.MessageOverlayVisible = true;
        this.SelectedTraderMessage = traderMessage;
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        this.MessageOverlayVisible = false;
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
    }

    private async void SearchTextBox_TextChanged(object _, string e)
    {
        if (e.IsNullOrWhiteSpace())
        {
            this.InitializeLatestTrades();
            return;
        }

        this.cancellationTokenSource?.Cancel();
        var trades = await this.TradeChatService.GetTradesByQuery(e, CancellationToken.None);
        await this.SetTrades(trades, false);
    }

    private void InitializeLatestTrades()
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = new CancellationTokenSource();
        this.GetLatestTrades();
        this.InitializeLiveData();
        this.InitializeDispatcherTimer();
    }

    private void InitializeDispatcherTimer()
    {
        this.dispatcherTimer.Interval = this.timeUpdateInterval;
        this.dispatcherTimer.Start();
    }

    private async void GetLatestTrades()
    {
        var response = await this.TradeChatService.GetLatestTrades(this.cancellationTokenSource?.Token ?? CancellationToken.None);
        await this.SetTrades(response, true);
    }

    private async void InitializeLiveData()
    {
        await this.GetLiveData(this.cancellationTokenSource?.Token ?? CancellationToken.None);
    }

    private async Task GetLiveData(CancellationToken cancellationToken)
    {
        await foreach(var tradeMessage in this.TradeChatService.GetLiveTraderMessages(cancellationToken))
        {
            var wrapper = new TraderMessageViewWrapper
            {
                UpdateTimer = this.dispatcherTimer,
                TraderMessage = tradeMessage
            };

            await this.Dispatcher.InvokeAsync(() => this.TraderMessages.Insert(0, wrapper));
        }
    }

    private async Task SetTrades(IEnumerable<TraderMessage> traderMessages, bool animate)
    {
        await this.Dispatcher.InvokeAsync(() =>
        {
            this.TraderMessages.Clear();
            foreach (var traderMessage in traderMessages)
            {
                this.TraderMessages.Add(new TraderMessageViewWrapper
                {
                    UpdateTimer = this.dispatcherTimer,
                    TraderMessage = traderMessage,
                    Initialized = !animate
                });
            }
        });
    }
}
