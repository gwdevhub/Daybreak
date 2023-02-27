using Daybreak.Models.Guildwars;
using Daybreak.Services.IconRetrieve;
using System;
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

    private IIconCache? iconRetriever;

    [GenerateDependencyProperty]
    private ImageSource imageSource = default!;
    [GenerateDependencyProperty]
    private double borderOpacity;

    public SkillTemplate()
    {
        this.InitializeComponent();
        this.DataContextChanged += this.SkillTemplate_DataContextChanged;
    }

    public void InitializeSkillTemplate(IIconCache iconRetriever)
    {
        this.iconRetriever = iconRetriever;
        this.SkillTemplate_DataContextChanged(this, new DependencyPropertyChangedEventArgs(UserControl.DataContextProperty, null, this.DataContext));
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
                if (maybeUri.ExtractValue() is Uri uri)
                {
                    this.ImageSource = new BitmapImage(uri);
                }
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
