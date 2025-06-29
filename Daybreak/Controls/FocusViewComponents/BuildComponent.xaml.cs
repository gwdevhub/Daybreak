using Daybreak.Controls.Buttons;
using Daybreak.Shared;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.FocusView;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.FocusViewComponents;
/// <summary>
/// Interaction logic for BuildComponent.xaml
/// </summary>
public partial class BuildComponent : UserControl
{
    private const string PveMetaBuildsUrl = "https://gwpvx.fandom.com/wiki/Category:Great_working_general_builds";

    private readonly INotificationService notificationService;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IAttachedApiAccessor attachedApiAccessor;
    private readonly IViewManager viewManager;

    private bool buildsInitialized = false;
    private List<IBuildEntry>? cachedBuilds;
    private MainPlayerBuildContext? cachedMainPlayerBuildContext;

    [GenerateDependencyProperty]
    private bool loading;

    [GenerateDependencyProperty]
    private bool validData;

    public event EventHandler<string>? NavigateToClicked;

    public ObservableCollection<IBuildEntry> Builds { get; } = [];

    public BuildComponent()
        : this(Global.GlobalServiceProvider.GetRequiredService<INotificationService>(),
               Global.GlobalServiceProvider.GetRequiredService<IViewManager>(),
               Global.GlobalServiceProvider.GetRequiredService<IAttachedApiAccessor>(),
               Global.GlobalServiceProvider.GetRequiredService<IBuildTemplateManager>())
    { 
    }

    public BuildComponent(
        INotificationService notificationService,
        IViewManager viewManager,
        IAttachedApiAccessor attachedApiAccessor,
        IBuildTemplateManager buildTemplateManager)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.attachedApiAccessor = attachedApiAccessor.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void UserControl_Loaded(object _, RoutedEventArgs __)
    {
        this.cachedBuilds = await this.buildTemplateManager.GetBuilds().ToListAsync();
        this.buildsInitialized = true;
    }

    private void UserControl_Unloaded(object _, RoutedEventArgs __)
    {
        this.Builds.Clear();
        this.cachedMainPlayerBuildContext = default;
        this.buildsInitialized = false;
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not BuildComponentContext context)
        {
            return;
        }

        if (!this.BuildContextChanged(context))
        {
            return;
        }

        this.Loading = true;
        this.cachedMainPlayerBuildContext = new MainPlayerBuildContext(context.PrimaryProfessionId, context.UnlockedProfessions, context.CharacterUnlockedSkills, context.AccountUnlockedSkills);
        this.LoadBuilds();
        this.Loading = false;
    }

    private async void GetPlayerBuildButton_Clicked(object _, EventArgs __)
    {
        if (this.attachedApiAccessor.ApiContext is not ScopedApiContext apiContext)
        {
            throw new InvalidOperationException("Attached API context is not available. Focus view should not be accessible without an attached context");
        }

        var buildEntry = await apiContext.GetMainPlayerBuild(CancellationToken.None);
        if (buildEntry is null)
        {
            this.notificationService.NotifyError(
                "Failed to load player build",
                "Could not retrieve the player's build from the API. Please check logs for more details");
            return;
        }

        try
        {
            var singleBuildEntry = this.buildTemplateManager.CreateSingleBuild(buildEntry);
            this.viewManager.ShowView<SingleBuildTemplateView>(singleBuildEntry);
        }
        catch
        {
            throw;
        }
    }

    private async void GetTeamBuildButton_Clicked(object _, EventArgs __)
    {
        if (this.attachedApiAccessor.ApiContext is not ScopedApiContext apiContext)
        {
            throw new InvalidOperationException("Attached API context is not available. Focus view should not be accessible without an attached context");
        }

        var partyLoadout = await apiContext.GetPartyLoadout(CancellationToken.None);
        if (partyLoadout is null)
        {
            this.notificationService.NotifyError(
                "Failed to load team build",
                "Could not retrieve the team build from the API. Please check logs for more details");
            return;
        }

        try
        {
            var teamBuildEntry = this.buildTemplateManager.CreateTeamBuild(partyLoadout);
            this.viewManager.ShowView<TeamBuildTemplateView>(teamBuildEntry);
        }
        catch
        {
            throw;
        }
    }

    private void PveMetaBuildsButton_Clicked(object _, EventArgs __)
    {
        this.NavigateToClicked?.Invoke(this, PveMetaBuildsUrl);
    }

    private async void LoadSingleBuildButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not BackButton backButton ||
            backButton.DataContext is not SingleBuildEntry build ||
            this.attachedApiAccessor.ApiContext is not ScopedApiContext apiContext)
        {
            return;
        }

        this.Loading = true;
        var code = this.buildTemplateManager.EncodeTemplate(build);
        await apiContext.PostMainPlayerBuild(code, CancellationToken.None).ConfigureAwait(true);
        this.Loading = false;
    }

    private async void LoadTeamBuildButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not BackButton backButton ||
            backButton.DataContext is not TeamBuildEntry build ||
            this.attachedApiAccessor.ApiContext is not ScopedApiContext apiContext)
        {
            return;
        }

        this.Loading = true;
        var loadout = this.buildTemplateManager.ConvertToPartyLoadout(build);
        await apiContext.PostPartyLoadout(loadout, CancellationToken.None).ConfigureAwait(true);
        this.Loading = false;
    }

    private void LoadBuilds()
    {
        if (this.cachedMainPlayerBuildContext is null)
        {
            this.Builds.Clear();
            return;
        }

        if (this.cachedBuilds is null)
        {
            this.Builds.Clear();
            return;
        }

        var validBuilds = this.cachedBuilds
            .Where(build =>
            {
                if (build is SingleBuildEntry singleBuild)
                {
                    return this.buildTemplateManager.CanApply(this.cachedMainPlayerBuildContext, singleBuild);
                }
                else if (build is TeamBuildEntry teamBuild)
                {
                    return this.buildTemplateManager.CanApply(this.cachedMainPlayerBuildContext, teamBuild);
                }
                else
                {
                    return false;
                }
            })
            .OrderBy(b => b.Name);

        this.UpdateBuildCollection(validBuilds);
    }

    private void UpdateBuildCollection(IEnumerable<IBuildEntry> validBuilds)
    {
        var buildsToAdd = validBuilds.Where(b1 => !this.Builds.Any(b2 => b1.Name == b2.Name)).ToList();
        var buildsToRemove = this.Builds.Where(b1 => !validBuilds.Any(b2 => b1.Name == b2.Name)).ToList();

        foreach(var build in buildsToRemove)
        {
            this.Builds.Remove(build);
        }

        foreach (var build in buildsToAdd)
        {
            this.Builds.Add(build);
        }
    }

    private bool BuildContextChanged(BuildComponentContext context)
    {
        if (!this.buildsInitialized)
        {
            return false;
        }

        if (this.cachedMainPlayerBuildContext is null)
        {
            return true;
        }
        else if (this.cachedMainPlayerBuildContext.PrimaryProfessionId != context.PrimaryProfessionId ||
            this.cachedMainPlayerBuildContext.UnlockedProfessions != context.UnlockedProfessions ||
            !this.cachedMainPlayerBuildContext.UnlockedCharacterSkills.SequenceEqual(context.CharacterUnlockedSkills) ||
            !this.cachedMainPlayerBuildContext.UnlockedAccountSkills.SequenceEqual(context.AccountUnlockedSkills))
        {
            return true;
        }

        return false;
    }
}
