using System.ComponentModel;
using System.Windows.Threading;

namespace Daybreak.Shared.Models.Trade;

public sealed class TraderMessageViewWrapper : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public IEnumerable<ColoredTextElement>? ColoredTextElements
    {
        get;
        init
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ColoredTextElements)));
        }
    }

    public TraderMessage? TraderMessage
    {
        get;
        init
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TraderMessage)));
        }
    }

    public DispatcherTimer? UpdateTimer
    {
        get;
        init
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UpdateTimer)));
        }
    }

    public bool Initialized
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Initialized)));
        }
    }

    public void TriggerTraderMessageRebinding()
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TraderMessage)));
    }
}
