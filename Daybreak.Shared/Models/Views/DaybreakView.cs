using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Daybreak.Shared.Models.Views;
public abstract class DaybreakView<TView, TViewModel> : ComponentBase, INotifyPropertyChanged
    where TView : DaybreakView<TView, TViewModel>
    where TViewModel : DaybreakViewModel<TViewModel, TView>
{
    public readonly static TimeSpan InitializationTimeout = TimeSpan.FromSeconds(5);

    public event PropertyChangedEventHandler? PropertyChanged;

    [Inject]
    public TViewModel ViewModel { get; set; } = default!;
    [Inject]
    protected ILogger<TView> Logger { get; set; } = default!;
    [Parameter]
    public object? DataContext
    {
        get => field;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DataContext)));
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (!firstRender)
        {
            return;
        }
        
        if (this is not TView view)
        {
            throw new InvalidOperationException($"View is not of type {typeof(TView).Name}. Actual type: {this.GetType().Name}");
        }

        this.ViewModel.SetView(view);
        using var initializationCts = new CancellationTokenSource(InitializationTimeout);
        _ = this.ViewModel ?? throw new InvalidOperationException("Cannot initialize ViewModel. ViewModel is null");
        try
        {
            await this.ViewModel.Initialize(initializationCts.Token);
        }
        catch(Exception ex)
        {
            this.Logger?.LogError(ex, "Failed to initialize ViewModel {ViewModelType}", typeof(TViewModel).Name);
        }
    }

    internal void RefreshView()
    {
        this.StateHasChanged();
    }
}
