using System.ComponentModel;
using System.Windows.Threading;

namespace Daybreak.Models.Trade;

public sealed class TraderMessageViewWrapper : INotifyPropertyChanged
{
    private TraderMessage? traderMessage;
    private DispatcherTimer? updateTimer;
    private bool initialized;

    public event PropertyChangedEventHandler? PropertyChanged;

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
