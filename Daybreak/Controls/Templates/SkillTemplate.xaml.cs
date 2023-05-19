using Daybreak.Launch;
using Daybreak.Models.Guildwars;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.Images;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for SkillTemplate.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
public partial class SkillTemplate : UserControl
{
    public event EventHandler<RoutedEventArgs>? Clicked;
    public event EventHandler? RemoveClicked;

    private IImageCache imageCache;
    private IIconCache iconRetriever;

    [GenerateDependencyProperty]
    private ImageSource imageSource = default!;
    [GenerateDependencyProperty]
    private double borderOpacity;

    public SkillTemplate()
        : this(Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IImageCache>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IIconCache>())
    {
    }

    public SkillTemplate(
        IImageCache imageCache,
        IIconCache iconCache)
    {
        this.imageCache = imageCache.ThrowIfNull();
        this.iconRetriever = iconCache.ThrowIfNull();
        this.InitializeComponent();
        this.DataContextChanged += this.SkillTemplate_DataContextChanged;
    }

    private async void SkillTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (this.iconRetriever is null)
        {
            return;
        }

        if (e.NewValue is Skill skill)
        {
            if (skill != Skill.NoSkill)
            {
                var maybeUri = await this.iconRetriever.GetIconUri(skill).ConfigureAwait(true);
                this.ImageSource = this.imageCache.GetImage(maybeUri);
            }
            else if (this.ImageSource is not null)
            {
                this.ImageSource = null;
            }
        }
    }

    private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.BorderOpacity = 1;
        this.CancelButton.Visibility = this.HasSkill() ? Visibility.Visible : Visibility.Hidden;
    }
    private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.BorderOpacity = 0;
        this.CancelButton.Visibility = Visibility.Hidden;
    }
    private void Rectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        this.RemoveClicked?.Invoke(this, e);
    }

    private bool HasSkill()
    {
        if (this.DataContext is Skill skill)
        {
            return skill != Skill.NoSkill;
        }

        return false;
    }
}
