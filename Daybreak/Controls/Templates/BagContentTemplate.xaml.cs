using Daybreak.Launch;
using Daybreak.Models.Guildwars;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.Images;
using Microsoft.Extensions.DependencyInjection;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Controls.Templates;

/// <summary>
/// Interaction logic for BagContentTemplate.xaml
/// </summary>
public partial class BagContentTemplate : UserControl
{
    private readonly IImageCache imageCache;
    private readonly IIconCache iconCache;

    private IBagContent? cachedBagContent;
    private string? cachedIconUri = default!;

    [GenerateDependencyProperty]
    private ImageSource imageSource = default!;

    [GenerateDependencyProperty]
    private bool iconVisible;

    [GenerateDependencyProperty]
    private bool contentVisible;

    [GenerateDependencyProperty]
    private int itemId;

    [GenerateDependencyProperty]
    private int count;

    public BagContentTemplate()
        : this(
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IImageCache>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IIconCache>())
    {
    }

    public BagContentTemplate(
        IImageCache imageCache,
        IIconCache iconCache)
    {
        this.imageCache = imageCache.ThrowIfNull();
        this.iconCache = iconCache.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not IBagContent bagContent)
        {
            this.cachedBagContent = default;
            this.ContentVisible = false;
            this.ImageSource = default;
            this.cachedIconUri = default;
            return;
        }

        if (this.BagContentAlreadyLoaded(bagContent))
        {
            this.ContentVisible = true;
            return;
        }

        if (bagContent is not BagItem bagItem)
        {
            if (bagContent is UnknownBagItem unknownBagItem)
            {
                this.cachedBagContent = unknownBagItem;
                this.ContentVisible = true;
                this.IconVisible = false;
                this.ItemId = (int)unknownBagItem.ItemId;
                this.Count = (int)unknownBagItem.Count;
            }

            return;
        }

        this.cachedBagContent = bagItem;
        this.ContentVisible = true;
        this.ItemId = bagItem.Item.Id;
        this.Count = (int)bagItem.Count;
        var maybeIconUri = await this.iconCache.GetIconUri(bagItem.Item);
        if (maybeIconUri is not string iconUri ||
            this.cachedIconUri == iconUri)
        {
            this.IconVisible = this.cachedIconUri == maybeIconUri;
            return;
        }

        this.IconVisible = true;
        this.cachedIconUri = iconUri;
        await this.Dispatcher.InvokeAsync(() =>
        {
            this.ImageSource = this.imageCache.GetImage(iconUri);
            
        });
    }

    private bool BagContentAlreadyLoaded(IBagContent bagContent)
    {
        if (this.cachedBagContent is null)
        {
            return false;
        }

        if (bagContent is BagItem newBagItem &&
            this.cachedBagContent is BagItem oldBagItem)
        {
            return newBagItem.Slot == oldBagItem.Slot &&
                newBagItem.Count == oldBagItem.Count &&
                newBagItem.Item == oldBagItem.Item;
        }

        if (bagContent is UnknownBagItem newUnknownBagItem &&
            this.cachedBagContent is UnknownBagItem oldUnknownBagItem)
        {
            return newUnknownBagItem.ItemId == oldUnknownBagItem.ItemId &&
                newUnknownBagItem.Count == oldUnknownBagItem.Count &&
                newUnknownBagItem.Slot == oldUnknownBagItem.Slot;
        }

        return false;
    }
}
