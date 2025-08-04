using Daybreak.Controls.Buttons;
using Daybreak.Shared.Models.Options;
using Daybreak.Shared.Services.Options;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Views;
/// <summary>
/// Interaction logic for OptionSectionView.xaml
/// </summary>
public partial class OptionSectionView : UserControl
{
    //private readonly IViewManager viewManager;
    private readonly IOptionsProvider optionsProvider;

    private object currentOptions = new();

    [GenerateDependencyProperty]
    private string title = string.Empty;

    public ObservableCollection<OptionEntry> OptionEntries { get; } = [];

    public OptionSectionView(
        //IViewManager viewManager,
        IOptionsProvider optionsProvider)
    {
        //this.viewManager = viewManager.ThrowIfNull();
        this.optionsProvider = optionsProvider.ThrowIfNull();

        this.InitializeComponent();

        this.DataContextChanged += this.OptionSectionView_DataContextChanged;
    }

    private void OptionSectionView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        this.optionsProvider.SaveRegisteredOptions(this.currentOptions);
        //this.viewManager.ShowView<LauncherView>();
    }

    private void CustomSetterButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not HighlightButton highlightButton ||
            highlightButton.DataContext is not OptionSetter optionSetter)
        {
            return;
        }

        if (!optionSetter.HasCustomSetter)
        {
            return;
        }

        //this.viewManager.ShowView(optionSetter.CustomSetterViewType!);
    }

    private void HelpButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not HelpButton helpButton)
        {
            return;
        }

        helpButton.ToolTip.As<ToolTip>()!.IsOpen = true;
        helpButton.ToolTip.As<ToolTip>()!.Content = helpButton.DataContext.As<OptionEntry>()?.Heading?.Description;
    }

    private void HelpButton_MouseLeave(object sender, MouseEventArgs e)
    {
        if (sender is not HelpButton helpButton)
        {
            return;
        }

        if (helpButton.ToolTip.As<ToolTip>()!.IsOpen)
        {
            helpButton.ToolTip.As<ToolTip>()!.IsOpen = false;
        }
    }
}
