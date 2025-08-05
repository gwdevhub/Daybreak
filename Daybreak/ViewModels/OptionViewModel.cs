using Daybreak.Shared.Models.Options;
using Daybreak.Shared.Services.Options;
using Daybreak.Views;
using TrailBlazr.ViewModels;

namespace Daybreak.ViewModels;
public sealed class OptionViewModel(IOptionsProvider optionsProvider)
    : ViewModelBase<OptionViewModel, OptionView>
{
    private readonly IOptionsProvider optionsProvider = optionsProvider;

    public OptionInstance? OptionInstance { 
        get => field;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.OptionInstance));
        }
    }

    public override ValueTask ParametersSet(OptionView view, CancellationToken cancellationToken)
    {
        this.OptionInstance = this.optionsProvider.GetRegisteredOptionInstance(view.OptionName ?? string.Empty);
        return base.ParametersSet(view, cancellationToken);
    }
}
