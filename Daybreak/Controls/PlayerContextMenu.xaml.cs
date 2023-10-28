using Daybreak.Models.FocusView;
using Daybreak.Models.Guildwars;
using Daybreak.Services.Scanner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls;
/// <summary>
/// Interaction logic for PlayerContextMenu.xaml
/// </summary>
public partial class PlayerContextMenu : UserControl
{
    private readonly IGuildwarsMemoryReader guildwarsMemoryReader;

    public event EventHandler<(PlayerInformation? Player, string? Name)>? PlayerContextMenuClicked;

    [GenerateDependencyProperty]
    private string playerName = string.Empty;

    public PlayerContextMenu()
        : this(Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IGuildwarsMemoryReader>())
    {
    }

    public PlayerContextMenu(
        IGuildwarsMemoryReader guildwarsMemoryReader)
    {
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.InitializeComponent();
        this.DataContextChanged += this.PlayerContextMenu_DataContextChanged;
    }

    private async void PlayerContextMenu_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (this.DataContext is not PlayerContextMenuContext context)
        {
            return;
        }

        await this.guildwarsMemoryReader.EnsureInitialized(context.GuildWarsApplicationLaunchContext!.GuildWarsProcess, CancellationToken.None);
        this.PlayerName = await this.guildwarsMemoryReader.GetNamedEntity(context.Player!, CancellationToken.None);
    }

    private void TextBlock_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        this.PlayerContextMenuClicked?.Invoke(this, (this.DataContext.As<PlayerContextMenuContext>()?.Player ?? default, this.PlayerName));
    }
}
