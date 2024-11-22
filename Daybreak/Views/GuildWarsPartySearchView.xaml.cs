using Daybreak.Services.Menu;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views;
/// <summary>
/// Interaction logic for GuildWarsPartySearchView.xaml
/// </summary>
public partial class GuildWarsPartySearchView : UserControl
{
    private readonly IMenuService menuService;

    public GuildWarsPartySearchView(
        IMenuService menuService)
    {
        this.menuService = menuService.ThrowIfNull();
        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.menuService.CloseMenu();
    }
}
