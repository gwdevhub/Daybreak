using Daybreak.Models.Guildwars;
using Daybreak.Services.Scanner;
using Daybreak.Services.TradeChat;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Core.Extensions;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls.Templates;
/// <summary>
/// Interaction logic for BagItemMenu.xaml
/// </summary>
public partial class BagItemMenu : UserControl
{
    private readonly IGuildwarsMemoryReader guildwarsMemoryReader;
    private readonly IItemHashService itemHashService;

    public event EventHandler? CloseClicked;
    public event EventHandler<ItemBase>? ItemWikiClicked;

    [GenerateDependencyProperty]
    private string itemName = string.Empty;
    [GenerateDependencyProperty]
    private string modHash = string.Empty;

    public BagItemMenu()
        : this(Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IGuildwarsMemoryReader>(),
              Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IItemHashService>())
    {
    }

    private BagItemMenu(
        IGuildwarsMemoryReader guildwarsMemoryReader,
        IItemHashService itemHashService)
    {
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.itemHashService = itemHashService.ThrowIfNull();
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.CloseClicked?.Invoke(this, e);
    }

    private async void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs e)
    {
        if (this.DataContext is not IBagContent content)
        {
            return;
        }

        this.ModHash = this.itemHashService.ComputeHash(content);
        var name = await this.guildwarsMemoryReader.GetItemName(
            content is BagItem bagItem ? bagItem.Item.Id :
            content is UnknownBagItem unknownBagItem ? (int)unknownBagItem.ItemId :
            0,
            content.Modifiers.Select(mod => mod.Modifier).ToList(),
            CancellationToken.None).ConfigureAwait(true);
        if (name is not null)
        {
            this.ItemName = name;
        }
    }

    private void WikiTextBlock_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        if (this.DataContext is not IBagContent content)
        {
            return;
        }

        if (content is not BagItem bagItem)
        {
            return;
        }

        this.ItemWikiClicked?.Invoke(this, bagItem.Item);
    }
}
