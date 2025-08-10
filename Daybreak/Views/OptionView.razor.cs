using Daybreak.Shared.Models.Options;
using Daybreak.Shared.Services.Options;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;

public sealed class OptionViewModel(IOptionsProvider optionsProvider)
    : ViewModelBase<OptionViewModel, OptionView>
{
    private readonly IOptionsProvider optionsProvider = optionsProvider;

    public OptionInstance? OptionInstance
    {
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

    public void OnValueChanged()
    {
        if (this.OptionInstance is null)
        {
            return;
        }

        this.optionsProvider.SaveRegisteredOptions(this.OptionInstance);
    }
}
