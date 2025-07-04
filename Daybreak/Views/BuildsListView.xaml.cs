﻿using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Toolbox;
using Daybreak.Shared.Utils;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for BuildsListView.xaml
/// </summary>
public partial class BuildsListView : UserControl
{
    private readonly IToolboxService toolboxService;
    private readonly IViewManager viewManager;
    private readonly IBuildTemplateManager buildTemplateManager;

    private IEnumerable<IBuildEntry>? buildEntries;

    [GenerateDependencyProperty]
    private bool loading;

    public SortedObservableCollection<IBuildEntry, string> BuildEntries { get; } = new SortedObservableCollection<IBuildEntry, string>(entry => entry.Name!);

    public BuildsListView(
        IViewManager viewManager,
        IToolboxService toolboxService,
        IBuildTemplateManager buildTemplateManager)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.toolboxService = toolboxService.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.InitializeComponent();
        this.LoadBuilds();
    }

    private async void LoadBuilds()
    {
        this.Loading = true;
        this.buildEntries = await this.buildTemplateManager.GetBuilds().ToListAsync();
        var toolboxBuildEntries = await this.toolboxService.GetToolboxBuilds(CancellationToken.None).ToListAsync();
        this.BuildEntries.ClearAnd().AddRange(this.buildEntries.OrderBy(b => b.Name)).AddRange(toolboxBuildEntries.OrderBy(b => b.Name));
        this.Loading = false;
        this.SearchTextBox.FocusOnTextBox();
    }

    private void AddSingleButton_Clicked(object sender, EventArgs e)
    {
        var build = this.buildTemplateManager.CreateSingleBuild();
        this.viewManager.ShowView<SingleBuildTemplateView>(build);
    }

    private void AddTeamButton_Clicked(object sender, EventArgs e)
    {
        var build = this.buildTemplateManager.CreateTeamBuild();
        this.viewManager.ShowView<TeamBuildTemplateView>(build);
    }

    private void BuildEntryTemplate_RemoveClicked(object _, IBuildEntry e)
    {
        if (e is TeamBuildEntry teamBuild &&
            teamBuild.IsToolboxBuild)
        {
            this.toolboxService.DeleteToolboxBuild(teamBuild, CancellationToken.None);
        }
        else
        {
            this.buildTemplateManager.RemoveBuild(e);
        }

        this.LoadBuilds();
    }

    private void SearchTextBox_TextChanged(object _, string e)
    {
        var selectedEntries = this.buildEntries!.Where(b => StringUtils.MatchesSearchString(b.Name!, e));

        var entriesToRemove = this.BuildEntries.Except(selectedEntries).ToList();
        var entriesToAdd = selectedEntries.Except(this.BuildEntries).ToList();
        foreach(var buildEntry in  entriesToRemove)
        {
            this.BuildEntries.Remove(buildEntry);
        }

        foreach(var buildEntry in entriesToAdd)
        {
            this.BuildEntries.Add(buildEntry);
        }
    }

    private void SynchronizeButton_Clicked(object _, EventArgs __)
    {
        this.viewManager.ShowView<BuildsSynchronizationView>();
    }

    private void BuildEntryTemplate_EntryClicked(object _, IBuildEntry e)
    {
        if (e is SingleBuildEntry)
        {
            this.viewManager.ShowView<SingleBuildTemplateView>(e);
        }
        else if (e is TeamBuildEntry)
        {
            this.viewManager.ShowView<TeamBuildTemplateView>(e);
        }
    }
}
