using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Templates;
/// <summary>
/// Interaction logic for PartyMemberTemplate.xaml
/// </summary>
public partial class PartyMemberTemplate : UserControl
{
    public event EventHandler<HeroBehavior>? BehaviorChanged;
    public event EventHandler<IBuildEntry>? BuildSelected;

    [GenerateDependencyProperty]
    private bool showBehavior = false;

    [GenerateDependencyProperty]
    private bool showName = false;

    [GenerateDependencyProperty]
    private HeroBehavior selectedBehavior;

    public ObservableCollection<HeroBehavior> HeroBehaviors { get; } =
    [
        HeroBehavior.Fight,
        HeroBehavior.Guard,
        HeroBehavior.Avoid
    ];

    public PartyMemberTemplate()
    {
        this.InitializeComponent();
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not PartyMemberEntry entry)
        {
            return;
        }

        this.ShowBehavior = entry.Hero is not null && entry.Hero != Hero.None;
        this.SelectedBehavior = entry.Behavior;
        this.ShowName = entry.Hero is not null && entry.Hero != Hero.None;
    }

    private void DropDownButton_SelectionChanged(object _, object newValue)
    {
        if (newValue is not HeroBehavior behavior)
        {
            return;
        }

        this.BehaviorChanged?.Invoke(this, behavior);
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        if (this.DataContext is not PartyMemberEntry entry)
        {
            return;
        }

        this.BuildSelected?.Invoke(this, entry.Build);
    }
}
