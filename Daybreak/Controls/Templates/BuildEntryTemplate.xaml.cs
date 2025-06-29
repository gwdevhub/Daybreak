using Daybreak.Shared.Models.Builds;
using System.Windows.Controls;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for BuildEntryTemplate.xaml
/// </summary>
public partial class BuildEntryTemplate : UserControl
{
    public event EventHandler<IBuildEntry>? RemoveClicked;
    public event EventHandler<IBuildEntry>? EntryClicked;

    public BuildEntryTemplate()
    {
        this.InitializeComponent();
    }

    private void BinButton_Clicked(object _, EventArgs __)
    {
        if (this.DataContext is IBuildEntry buildEntry)
        {
            this.RemoveClicked?.Invoke(this, buildEntry);
        }
    }

    private void HighlightButton_Clicked(object _, EventArgs __)
    {
        if (this.DataContext is IBuildEntry buildEntry)
        {
            this.EntryClicked?.Invoke(this, buildEntry);
        }
    }
}
