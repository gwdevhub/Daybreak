using Daybreak.Shared.Models.Guildwars;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls.FocusViewComponents;
/// <summary>
/// Interaction logic for TitleInformationComponent.xaml
/// </summary>
public partial class TitleInformationComponent : UserControl
{
    public event EventHandler<string>? NavigateToClicked;

    [GenerateDependencyProperty]
    private string titleRankName = string.Empty;

    public TitleInformationComponent()
    {
        this.InitializeComponent();
    }

    private void Title_MouseLeftButtonDown(object _, MouseButtonEventArgs __)
    {
        if (this.DataContext is not TitleInformation titleInformation ||
            titleInformation.Title is not Title title ||
            title.WikiUrl is not string url)
        {
            return;
        }

        this.NavigateToClicked?.Invoke(this, url);
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not TitleInformation titleInformation)
        {
            return;
        }

        if (titleInformation.Title is not null &&
                titleInformation.Title.Tiers!.Count > titleInformation.TierNumber - 1)
        {
            var rankIndex = (int)titleInformation.TierNumber! - 1;
            this.TitleRankName = $"{titleInformation.Title.Tiers![rankIndex]} ({titleInformation.TierNumber}/{titleInformation.MaxTierNumber})";
        }
        else
        {
            this.TitleRankName = string.Empty;
        }
    }
}
