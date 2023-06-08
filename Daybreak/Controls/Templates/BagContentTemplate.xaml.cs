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

    private string? cachedIconUri = default!;

    [GenerateDependencyProperty]
    private ImageSource imageSource = default!;

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
            this.ImageSource = default;
            this.cachedIconUri = default;
            return;
        }

        if (bagContent is not BagItem bagItem)
        {
            return;
        }

        var maybeIconUri = await this.iconCache.GetIconUri(bagItem.Item).ConfigureAwait(true);
        if (maybeIconUri is not string iconUri ||
            this.cachedIconUri == iconUri)
        {
            return;
        }

        this.ImageSource = this.imageCache.GetImage(iconUri);
        this.cachedIconUri = iconUri;
    }
}
