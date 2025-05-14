using Daybreak.Launch;
using Daybreak.Shared;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.IconRetrieve;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

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

    private IIconCache iconRetriever;

    [GenerateDependencyProperty]
    private string imageUri = string.Empty;
    [GenerateDependencyProperty]
    private double borderOpacity;

    public SkillTemplate()
        : this(Global.GlobalServiceProvider.GetRequiredService<IIconCache>())
    {
    }

    public SkillTemplate(
        IIconCache iconCache)
    {
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
                this.ImageUri = maybeUri;
            }
            else if (!this.ImageUri.IsNullOrWhiteSpace())
            {
                this.ImageUri = string.Empty;
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
