using Daybreak.Launch;
using Daybreak.Shared;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Scanner;
using Daybreak.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Core.Extensions;
using System.Threading;
using System.Windows.Controls;

namespace Daybreak.Controls.FocusViewComponents;
/// <summary>
/// Interaction logic for MainPlayerInformationComponent.xaml
/// </summary>
public partial class MainPlayerInformationComponent : UserControl
{
    private readonly IGuildwarsMemoryCache guildwarsMemoryCache;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IViewManager viewManager;

    public event EventHandler<string>? NavigateToClicked;

    public MainPlayerInformationComponent()
        : this(
              Global.GlobalServiceProvider.GetRequiredService<IGuildwarsMemoryCache>(),
              Global.GlobalServiceProvider.GetRequiredService<IBuildTemplateManager>(),
              Global.GlobalServiceProvider.GetRequiredService<IViewManager>())
    {
    }

    public MainPlayerInformationComponent(
        IGuildwarsMemoryCache guildwarsMemoryCache,
        IBuildTemplateManager buildTemplateManager,
        IViewManager viewManager)
    {
        this.guildwarsMemoryCache = guildwarsMemoryCache.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void MetaBuilds_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.DataContext is not MainPlayerInformation mainPlayer)
        {
            return;
        }

        if (mainPlayer.PrimaryProfession is not null &&
            mainPlayer.PrimaryProfession != Profession.None &&
            mainPlayer.PrimaryProfession.BuildsUrl is string url)
        {
            this.NavigateToClicked?.Invoke(this, url);
        }
    }

    private void PrimaryProfession_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.DataContext is not MainPlayerInformation mainPlayer)
        {
            return;
        }

        if (mainPlayer.PrimaryProfession is not null &&
            mainPlayer.PrimaryProfession != Profession.None &&
            mainPlayer.PrimaryProfession.WikiUrl is string url)
        {
            this.NavigateToClicked?.Invoke(this, url);
        }
    }

    private void SecondaryProfession_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.DataContext is not MainPlayerInformation mainPlayer)
        {
            return;
        }

        if (mainPlayer.SecondaryProfession is not null &&
            mainPlayer.SecondaryProfession != Profession.None &&
            mainPlayer.SecondaryProfession.WikiUrl is string url)
        {
            this.NavigateToClicked?.Invoke(this, url);
        }
    }

    private void EditBuild_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.DataContext is not MainPlayerInformation mainPlayer)
        {
            return;
        }

        if (mainPlayer.CurrentBuild is Build build)
        {
            var buildEntry = this.buildTemplateManager.CreateSingleBuild();
            buildEntry.Primary = build.Primary;
            buildEntry.Secondary = build.Secondary;
            buildEntry.Attributes = build.Attributes;
            buildEntry.Skills = build.Skills;
            this.viewManager.ShowView<SingleBuildTemplateView>(buildEntry);
        }
    }

    private async void EditTeamBuild_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        var teamBuildData = await this.guildwarsMemoryCache.ReadTeamBuildData(CancellationToken.None);
        if (teamBuildData is null)
        {
            return;
        }

        var teamBuild = this.buildTemplateManager.CreateTeamBuild(teamBuildData);
        this.viewManager.ShowView<TeamBuildTemplateView>(teamBuild);
    }
}
