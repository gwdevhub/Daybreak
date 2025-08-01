using System.ComponentModel;
using System.Core.Extensions;

namespace Daybreak.Shared.Models.Views;
public abstract class DaybreakViewModel<TViewModel, TView> : INotifyPropertyChanged
    where TViewModel : DaybreakViewModel<TViewModel, TView>
    where TView : DaybreakView<TView, TViewModel>
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public object? DataContext
    {
        get => field;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DataContext)));
        }
    }

    protected TView? View
    {
        get => field;
        private set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.View)));
        }
    }

    internal void SetView(TView view)
    {
        this.View = view.ThrowIfNull();
    }

    public void RefreshView()
    {
        this.View?.RefreshView();
    }

    public virtual ValueTask Initialize(CancellationToken cancellationToken) => ValueTask.CompletedTask;
}
