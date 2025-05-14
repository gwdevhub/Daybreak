using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;

namespace Daybreak.Shared.Models.Trade;

public sealed class TraderMessageViewWrapper : INotifyPropertyChanged
{
    private TraderMessage? traderMessage;
    private DispatcherTimer? updateTimer;
    private IEnumerable<ColoredTextElement>? coloredTextElements;
    private bool initialized;

    public event PropertyChangedEventHandler? PropertyChanged;

    public IEnumerable<ColoredTextElement>? ColoredTextElements
    {
        get => this.coloredTextElements;
        init
        {
            this.coloredTextElements = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ColoredTextElements)));
        }
    }

    public TraderMessage? TraderMessage
    {
        get => this.traderMessage;
        init
        {
            this.traderMessage = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TraderMessage)));
        }
    }

    public DispatcherTimer? UpdateTimer
    {
        get => this.updateTimer;
        init
        {
            this.updateTimer = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UpdateTimer)));
        }
    }

    public bool Initialized
    {
        get => this.initialized;
        set
        {
            this.initialized = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Initialized)));
        }
    }

    public void TriggerTraderMessageRebinding()
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TraderMessage)));
    }
}
