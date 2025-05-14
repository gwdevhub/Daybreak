using Daybreak.Shared.Models.Trade;
using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Daybreak.Controls.Templates;

/// <summary>
/// Interaction logic for TradeMessageTemplate.xaml
/// </summary>
public partial class TradeMessageTemplate : UserControl
{
    private DispatcherTimer? dispatcherTimer;
    private readonly Storyboard? storyboard;

    public event EventHandler<TraderMessage>? TraderMessageClicked;

    public TradeMessageTemplate()
    {
        this.InitializeComponent();
        this.storyboard = this.FindResource("FadeOut").As<Storyboard>();
    }

    private void UserControl_Loaded(object _, RoutedEventArgs __)
    {
        this.ApplyFadeOut();
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        /*
         * The data context should contain the trader message, an initialized property and a timer.
         * The initialized property shows if the message has been shown before or not, resulting in
         * animating the message.
         * The timer is used to tick the data context and update it at a specified interval.
         * 
         * To update the timer, first check if a timer already exists, remove any existing references,
         * update the timer to the new value, add new references to tick.
         */
        if (this.DataContext is not TraderMessageViewWrapper traderMessageViewWrapper)
        {
            return;
        }

        this.UpdateDispatcherTimer(traderMessageViewWrapper.UpdateTimer);
        if (traderMessageViewWrapper.Initialized)
        {
            return;
        }

        traderMessageViewWrapper.Initialized = true;
        this.BeginStoryboard(this.storyboard);
    }
   
    private void UpdateDispatcherTimer(DispatcherTimer? dispatcherTimer)
    {
        if (this.dispatcherTimer is null)
        {
            this.dispatcherTimer = dispatcherTimer;
        }
        else if (this.dispatcherTimer != dispatcherTimer)
        {
            this.dispatcherTimer.Tick -= this.RefreshDataContext;
        }

        if (this.dispatcherTimer is not null)
        {
            this.dispatcherTimer.Tick += this.RefreshDataContext;
        }
    }

    private void ApplyFadeOut()
    {
        if (this.DataContext is not TraderMessageViewWrapper traderMessageViewWrapper)
        {
            return;
        }

        if (traderMessageViewWrapper.Initialized)
        {
            return;
        }

        traderMessageViewWrapper.Initialized = true;
        this.BeginStoryboard(this.storyboard);
    }

    private void RefreshDataContext(object? _, EventArgs? __)
    {
        if (this.DataContext is not TraderMessageViewWrapper traderMessageViewWrapper)
        {
            return;
        }

        traderMessageViewWrapper.TriggerTraderMessageRebinding();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        if (this.DataContext is not TraderMessageViewWrapper traderMessageViewWrapper)
        {
            return;
        }

        this.TraderMessageClicked?.Invoke(this, traderMessageViewWrapper.TraderMessage!);
    }
}
