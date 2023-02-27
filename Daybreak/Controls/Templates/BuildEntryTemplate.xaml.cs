using Daybreak.Models.Builds;
using System;
using System.Windows.Controls;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for BuildEntryTemplate.xaml
/// </summary>
public partial class BuildEntryTemplate : UserControl
{
    public event EventHandler<BuildEntry>? RemoveClicked;

    public BuildEntryTemplate()
    {
        this.InitializeComponent();
    }

    private void BinButton_Clicked(object sender, EventArgs e)
    {
        if (this.DataContext is BuildEntry buildEntry)
        {
            this.RemoveClicked?.Invoke(this, buildEntry);
        }
    }
}
