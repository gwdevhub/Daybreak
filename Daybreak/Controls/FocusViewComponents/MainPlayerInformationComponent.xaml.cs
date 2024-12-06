using Daybreak.Launch;
using Daybreak.Models.Guildwars;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Navigation;
using Daybreak.Services.Scanner;
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
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IGuildwarsMemoryCache>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IBuildTemplateManager>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>())
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
        var teamBuild = this.buildTemplateManager.CreateTeamBuild();
        teamBuild.Builds.Clear();
        var mainPlayerBuild = this.buildTemplateManager.CreateSingleBuild();
        mainPlayerBuild.Primary = teamBuildData?.PlayerBuild?.Primary!;
        mainPlayerBuild.Secondary = teamBuildData?.PlayerBuild?.Secondary!;
        mainPlayerBuild.Attributes = teamBuildData?.PlayerBuild?.Attributes!;
        mainPlayerBuild.Skills = teamBuildData?.PlayerBuild?.Skills!;
        teamBuild.Builds.Add(mainPlayerBuild);
        foreach (var build in teamBuildData?.TeamMemberBuilds!)
        {
            if (build is null)
            {
                continue;
            }

            var singleBuildEntry = this.buildTemplateManager.CreateSingleBuild();
            singleBuildEntry.Primary = build.Primary;
            singleBuildEntry.Secondary = build.Secondary;
            singleBuildEntry.Attributes = build.Attributes;
            singleBuildEntry.Skills = build.Skills;
            teamBuild.Builds.Add(singleBuildEntry);
        }

        this.viewManager.ShowView<TeamBuildTemplateView>(teamBuild);
    }
}
