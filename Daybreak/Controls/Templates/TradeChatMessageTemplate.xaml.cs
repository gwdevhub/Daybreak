using Daybreak.Models.Trade;
using System;
using System.Windows.Controls;

namespace Daybreak.Controls.Templates;

/// <summary>
/// Interaction logic for TradeChatMessageTemplate.xaml
/// </summary>
public partial class TradeChatMessageTemplate : UserControl
{
    public event EventHandler<TraderMessage>? NameCopyClicked;
    public event EventHandler<TraderMessage>? MessageCopyClicked;
    public event EventHandler<TraderMessage>? TimestampCopyClicked;
    public event EventHandler? CloseButtonClicked;

    public TradeChatMessageTemplate()
    {
        this.InitializeComponent();
    }

    private void NameCopyButton_Clicked(object _, EventArgs e)
    {
        if (this.DataContext is not TraderMessage traderMessage)
        {
            return;
        }

        this.NameCopyClicked?.Invoke(this, traderMessage);
    }

    private void MessageCopyButton_Clicked(object _, EventArgs e)
    {
        if (this.DataContext is not TraderMessage traderMessage)
        {
            return;
        }

        this.MessageCopyClicked?.Invoke(this, traderMessage);
    }

    private void TimestampCopyButton_Clicked(object _, EventArgs e)
    {
        if (this.DataContext is not TraderMessage traderMessage)
        {
            return;
        }

        this.TimestampCopyClicked?.Invoke(this, traderMessage);
    }

    private void CloseButton_Clicked(object _, EventArgs e)
    {
        this.CloseButtonClicked?.Invoke(this, e);
    }
}
