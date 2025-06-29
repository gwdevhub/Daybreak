using Daybreak.Configuration.Options;
using Daybreak.Launch;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.FocusView;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Experience;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Services.Window;
using Daybreak.Views.Trade;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for FocusView.xaml
/// </summary>
public partial class FocusView : UserControl
{
    private const string NamePlaceholder = "[NamePlaceholder]";
    private const string WikiUrl = "https://wiki.guildwars.com/wiki/[NamePlaceholder]";
    private const int MaxRetries = 5;

    private static readonly TimeSpan UninitializedBackoff = TimeSpan.FromSeconds(15);
    private static readonly TimeSpan MainPlayerDataFrequency = TimeSpan.FromSeconds(1);

    private readonly IWindowEventsHook<MainWindow> mainWindowEventsHook;
    private readonly INotificationService notificationService;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IApplicationLauncher applicationLauncher;
    private readonly IExperienceCalculator experienceCalculator;
    private readonly IViewManager viewManager;
    private readonly IScreenManager screenManager;
    private readonly ILiveUpdateableOptions<FocusViewOptions> liveUpdateableOptions;
    private readonly ILiveUpdateableOptions<MinimapWindowOptions> minimapWindowOptions;
    private readonly ILogger<FocusView> logger;

    [GenerateDependencyProperty]
    private bool mainPlayerDataValid;

    [GenerateDependencyProperty]
    private string browserAddress = string.Empty;

    [GenerateDependencyProperty]
    private bool pauseDataFetching;

    [GenerateDependencyProperty]
    private CharacterComponentContext characterSelectComponentContext = default!;

    [GenerateDependencyProperty]
    private CurrentMapComponentContext currentMapComponentContext = default!;

    [GenerateDependencyProperty]
    private TitleInformationComponentContext titleInformationComponentContext = default!;

    [GenerateDependencyProperty]
    private QuestLogComponentContext questLogComponentContext = default!;

    [GenerateDependencyProperty]
    private PlayerResourcesComponentContext playerResourcesComponentContext = default!;

    [GenerateDependencyProperty]
    private VanquishComponentContext vanquishComponentContext = default!;

    [GenerateDependencyProperty]
    private BuildComponentContext buildComponentContext = default!;

    private bool browserMaximized = false;
    private CancellationTokenSource? cancellationTokenSource;

    public FocusView(
        IWindowEventsHook<MainWindow> mainWindowEventsHook,
        INotificationService notificationService,
        IBuildTemplateManager buildTemplateManager,
        IApplicationLauncher applicationLauncher,
        IExperienceCalculator experienceCalculator,
        IViewManager viewManager,
        IScreenManager screenManager,
        ILiveUpdateableOptions<FocusViewOptions> liveUpdateableOptions,
        ILiveUpdateableOptions<MinimapWindowOptions> minimapWindowOptions,
        ILogger<FocusView> logger)
    {
        this.mainWindowEventsHook = mainWindowEventsHook.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.experienceCalculator = experienceCalculator.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.screenManager = screenManager.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.minimapWindowOptions = minimapWindowOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == BrowserAddressProperty &&
            this.Browser.BrowserEnabled)
        {
            this.liveUpdateableOptions.Value.BrowserHistory = this.Browser.BrowserHistoryManager.BrowserHistory;
            this.liveUpdateableOptions.UpdateOption();
        }

        base.OnPropertyChanged(e);
    }

    private async void PeriodicallyReadMainPlayerContextData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var retries = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (this.DataContext is not FocusViewContext context)
                {
                    await Task.Delay(MainPlayerDataFrequency, cancellationToken);
                    continue;
                }

                if (context.LaunchContext.GuildWarsProcess.HasExited)
                {
                    this.logger.LogDebug("Executable is not running. Returning to {view}", nameof(LauncherView));
                    this.viewManager.ShowView<LauncherView>();
                    return;
                }

                if (this.PauseDataFetching)
                {
                    await Task.Delay(MainPlayerDataFrequency, cancellationToken);
                    continue;
                }

                var isAvailableTask = context.ApiContext.IsAvailable(cancellationToken);
                var instanceInfoTask = context.ApiContext.GetMainPlayerInstanceInfo(cancellationToken);
                await Task.WhenAny(isAvailableTask, instanceInfoTask);

                var isAvailable = await isAvailableTask;
                if (isAvailable is not true)
                {
                    retries++;
                    if (retries >= MaxRetries)
                    {
                        scopedLogger.LogError("Could not ensure connection is initialized. Returning to launcher view");
                        this.notificationService.NotifyError(
                            title: "Guild Wars unresponsive",
                            description: "Could not connect to Guild Wars instance. Returning to Launcher view");
                        this.viewManager.ShowView<LauncherView>();
                        break;
                    }
                    else
                    {
                        scopedLogger.LogError("Could not ensure connection is initialized. Backing off before retrying");
                        await Task.Delay(UninitializedBackoff, cancellationToken);
                    }

                    continue;
                }

                var instanceInfo = await instanceInfoTask;
                if (instanceInfo is null ||
                    instanceInfo.Type is Shared.Models.Api.InstanceType.Loading or Shared.Models.Api.InstanceType.Undefined)
                {
                    this.MainPlayerDataValid = false;
                    await Task.Delay(MainPlayerDataFrequency, cancellationToken);
                    continue;
                }

                var mainPlayerInfoTask = context.ApiContext.GetMainPlayerInfo(cancellationToken);
                var mainPlayerStateTask = context.ApiContext.GetMainPlayerState(cancellationToken);
                var mainPlayerBuildContextTask = context.ApiContext.GetMainPlayerBuildContext(cancellationToken);
                var characterSelectTask = context.ApiContext.GetCharacters(cancellationToken);
                var titleInfoTask = context.ApiContext.GetTitleInfo(cancellationToken);
                var questLogTask = context.ApiContext.GetMainPlayerQuestLog(cancellationToken);
                await Task.WhenAll(
                    mainPlayerInfoTask,
                    mainPlayerStateTask,
                    mainPlayerBuildContextTask,
                    characterSelectTask,
                    titleInfoTask,
                    questLogTask,
                    Task.Delay(MainPlayerDataFrequency, cancellationToken)).ConfigureAwait(true);

                var mainPlayerInfo = await mainPlayerInfoTask;
                var mainPlayerState = await mainPlayerStateTask;
                var mainPlayerBuildContext = await mainPlayerBuildContextTask;
                var characters = await characterSelectTask;
                var titleInfo = await titleInfoTask;
                var questLog = await questLogTask;

                if (mainPlayerInfo is null ||
                    mainPlayerState is null ||
                    characters is null ||
                    questLog is null ||
                    mainPlayerBuildContext is null)
                {
                    this.MainPlayerDataValid = false;
                    continue;
                }

                this.SetCurrentMapComponentContext(instanceInfo);
                this.SetTitleInformationComponentContext(titleInfo);
                this.SetCharacterSelectComponentContext(mainPlayerState, characters);
                this.SetQuestLogComponentContext(questLog);
                this.SetPlayerResourcesComponentContext(mainPlayerState);
                this.SetVanquishComponentContext(instanceInfo);
                this.SetBuildComponentContext(instanceInfo, mainPlayerBuildContext);

                this.MainPlayerDataValid = !this.PauseDataFetching;
                this.Browser.Visibility = Visibility.Visible;
                retries = 0;
            }
            catch (InvalidOperationException ex)
            {
                scopedLogger.LogError(ex, "Encountered invalid operation exception. Cancelling periodic main player reading");
                return;
            }
            catch (Exception ex)
            {
                scopedLogger.LogError(ex, "Encountered non-terminating exception. Silently continuing");
            }
        }
    }

    private void FocusView_Loaded(object _, RoutedEventArgs e)
    {
        if (this.DataContext is not FocusViewContext context)
        {
            return;
        }

        this.mainWindowEventsHook.RegisterHookOnSizeOrMoveBegin(this.OnMainWindowSizeOrMoveStart);
        this.mainWindowEventsHook.RegisterHookOnSizeOrMoveEnd(this.OnMainWindowSizeOrMoveEnd);
        this.Browser.BrowserHistoryManager.SetBrowserHistory(this.liveUpdateableOptions.Value.BrowserHistory);
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = this.cancellationTokenSource.Token;
        this.PeriodicallyReadMainPlayerContextData(cancellationToken);
    }

    private void FocusView_Unloaded(object _, RoutedEventArgs e)
    {
        this.mainWindowEventsHook.UnregisterHookOnSizeOrMoveBegin(this.OnMainWindowSizeOrMoveStart);
        this.mainWindowEventsHook.UnregisterHookOnSizeOrMoveEnd(this.OnMainWindowSizeOrMoveEnd);
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = null;
    }

    private void Browser_MaximizeClicked(object _, EventArgs e)
    {
        this.browserMaximized = !this.browserMaximized;
        if (this.browserMaximized)
        {
            this.BrowserHolder.Children.Remove(this.Browser);
            this.FullScreenHolder.Children.Add(this.Browser);
            this.FullScreenHolder.Visibility = Visibility.Visible;
        }
        else
        {
            this.FullScreenHolder.Children.Remove(this.Browser);
            this.BrowserHolder.Children.Add(this.Browser);
            this.FullScreenHolder.Visibility = Visibility.Hidden;
        }
    }

    private async void CharacterSelectComponent_SwitchCharacterClicked(object _, CharacterSelectComponentEntry? e)
    {
        if (this.DataContext is not FocusViewContext context ||
            context.ApiContext is not ScopedApiContext apiContext ||
            e.CharacterName.IsNullOrWhiteSpace())
        {
            return;
        }

        await this.Dispatcher.InvokeAsync(() =>
        {
            this.PauseDataFetching = true;
            this.MainPlayerDataValid = false;
        });
        await apiContext.SwitchCharacter(e.CharacterName, this.cancellationTokenSource?.Token ?? CancellationToken.None);
        await this.Dispatcher.InvokeAsync(() =>
        {
            this.PauseDataFetching = false;
            this.MainPlayerDataValid = true;
        });
    }

    private void Browser_BuildDecoded(object _, DownloadedBuild e)
    {
        if (e is null ||
            e.Build is null)
        {
            return;
        }

        e.Build.Name = e.PreferredName ?? e.Build.Name;
        if (e.Build is SingleBuildEntry)
        {
            this.viewManager.ShowView<SingleBuildTemplateView>(e.Build);
        }
        else if (e.Build is TeamBuildEntry)
        {
            this.viewManager.ShowView<TeamBuildTemplateView>(e.Build);
        }
        
    }

    private void Component_NavigateToClicked(object _, string e)
    {
        this.BrowserAddress = e;
    }

    private void InventoryComponent_ItemWikiClicked(object _, ItemBase e)
    {
        if (e is not IWikiEntity entity)
        {
            return;
        }

        this.BrowserAddress = entity.WikiUrl;
    }

    private void GuildwarsMinimap_NpcNameClicked(object _, string e)
    {
        if (e.IsNullOrEmpty() is not false)
        {
            return;
        }

        var indexOfSeparator = e.IndexOf("[");
        indexOfSeparator = indexOfSeparator >= 0 ? indexOfSeparator : e.Length;
        var curedNpcName = e[..indexOfSeparator];
        var npcUrl = WikiUrl.Replace(NamePlaceholder, curedNpcName);
        this.BrowserAddress = npcUrl;
    }

    private void InventoryComponent_PriceHistoryClicked(object _, ItemBase e)
    {
        this.viewManager.ShowView<PriceHistoryView>(e);
    }

    private void OnMainWindowSizeOrMoveStart()
    {
        this.IsEnabled = false;
        this.PauseDataFetching = true;
    }

    private void OnMainWindowSizeOrMoveEnd()
    {
        this.IsEnabled = true;
        this.PauseDataFetching = false;
    }

    private void OnMinimapWindowSizeOrMoveStart()
    {
        this.PauseDataFetching = true;
    }

    private void OnMinimapWindowSizeOrMoveEnd()
    {
        this.PauseDataFetching = false;
    }

    private void SetVanquishComponentContext(InstanceInfo instanceInfo)
    {
        this.VanquishComponentContext = new VanquishComponentContext
        {
            FoesKilled = instanceInfo.FoesKilled,
            FoesToKill = instanceInfo.FoesToKill,
            HardMode = instanceInfo.Difficulty is DifficultyInfo.Hard,
            Vanquishing = (instanceInfo.FoesToKill + instanceInfo.FoesKilled > 0U) && instanceInfo.Difficulty is DifficultyInfo.Hard
        };
    }

    private void SetCurrentMapComponentContext(InstanceInfo instanceInfo)
    {
        _ = Map.TryParse((int)instanceInfo.MapId, out var map);
        this.CurrentMapComponentContext = new CurrentMapComponentContext { CurrentMap = map };
    }

    private void SetCharacterSelectComponentContext(MainPlayerState state, CharacterSelectInformation characters)
    {
        var characterNames = characters.CharacterNames
                    .Select(c =>
                    {
                        _ = Profession.TryParse((int)c.Primary, out var primary);
                        _ = Profession.TryParse((int)c.Secondary, out var secondary);
                        var professionText = secondary == Profession.None ?
                            primary.Alias is null ? string.Empty : $"{primary.Alias} "
                            : $"{primary.Alias}/{secondary.Alias} ";
                        var name = $"{professionText}{c.Name}";
                        return (c, name);
                    })
                    .ToList();



        var mainCharacter = characterNames.FirstOrDefault(c => c.c.Name == characters.CurrentCharacter?.Name);
        var restCharacters = characterNames.Where(c => c.c.Name != mainCharacter.c.Name);
        this.CharacterSelectComponentContext = new CharacterComponentContext
        {
            CurrentExperience = state.CurrentExperience,
            Characters = [.. restCharacters.Select(c => new CharacterSelectComponentEntry { CharacterName = c.c.Name, DisplayName = c.name })],
            CurrentCharacter = new CharacterSelectComponentEntry { CharacterName = mainCharacter.c.Name, DisplayName = mainCharacter.name }
        };
    }

    private void SetQuestLogComponentContext(QuestLogInformation questLog)
    {
        var currentQuestEntry = questLog.Quests.FirstOrDefault(q => q.QuestId == questLog.CurrentQuestId);
        QuestMetadata? currentQuestMeta = default;
        if (currentQuestEntry is not null)
        {
            _ = Quest.TryParse((int)questLog.CurrentQuestId, out var currentQuest);
            _ = Map.TryParse((int)currentQuestEntry.MapFrom, out var mapFrom);
            _ = Map.TryParse((int)currentQuestEntry.MapTo, out var mapTo);
            currentQuestMeta = new QuestMetadata
            {
                From = mapFrom,
                To = mapTo,
                Quest = currentQuest
            };
        }

        this.QuestLogComponentContext = new QuestLogComponentContext
        {
            CurrentQuest = currentQuestMeta,
            Quests = [.. questLog.Quests
                .Where(q => q.QuestId != questLog.CurrentQuestId)
                .Select(quest =>
                {
                    if (!Quest.TryParse((int)quest.QuestId, out var parsedQuest) ||
                        !Map.TryParse((int)quest.MapFrom, out var mapFrom) ||
                        !Map.TryParse((int)quest.MapTo, out var mapTo))
                    {
                        return default;
                    }

                    return new QuestMetadata { From = mapFrom, To = mapTo, Quest = parsedQuest };
                })
                .OfType<QuestMetadata>()]
        };
    }

    private void SetTitleInformationComponentContext(TitleInfo? titleInfo)
    {
        if (titleInfo is not null &&
            Title.TryParse((int)titleInfo.Id, out var title))
        {
            this.TitleInformationComponentContext = new TitleInformationComponentContext
            {
                Title = title,
                CurrentPoints = titleInfo.CurrentPoints,
                IsPercentage = titleInfo.IsPercentage,
                MaxTierNumber = titleInfo.MaxTierNumber,
                TierNumber = titleInfo.TierNumber,
                PointsForCurrentRank = titleInfo.PointsForCurrentRank,
                PointsForNextRank = titleInfo.PointsForNextRank
            };
        }
        else
        {
            this.TitleInformationComponentContext = new TitleInformationComponentContext
            {
                Title = Title.None,
                CurrentPoints = 0,
                IsPercentage = false,
                MaxTierNumber = 0,
                TierNumber = 0,
                PointsForCurrentRank = 0,
                PointsForNextRank = 0
            };
        }
    }

    private void SetPlayerResourcesComponentContext(MainPlayerState state)
    {
        this.PlayerResourcesComponentContext = new PlayerResourcesComponentContext
        {
            CurrentBalthazar = state.CurrentBalthazar,
            CurrentImperial = state.CurrentImperial,
            CurrentLuxon = state.CurrentLuxon,
            CurrentKurzick = state.CurrentKurzick,

            MaxBalthazar = state.MaxBalthazar,
            MaxImperial = state.MaxImperial,
            MaxLuxon = state.MaxLuxon,
            MaxKurzick = state.MaxKurzick,

            TotalBalthazar = state.TotalBalthazar,
            TotalImperial = state.TotalImperial,
            TotalLuxon = state.TotalLuxon,
            TotalKurzick = state.TotalKurzick,
        };
    }

    private void SetBuildComponentContext(InstanceInfo instanceInfo, MainPlayerBuildContext mainPlayerBuildContext)
    {
        this.BuildComponentContext = new BuildComponentContext
        {
            IsInOutpost = instanceInfo.Type is Shared.Models.Api.InstanceType.Outpost,
            PrimaryProfessionId = mainPlayerBuildContext.PrimaryProfessionId,
            AccountUnlockedSkills = mainPlayerBuildContext.UnlockedAccountSkills,
            CharacterUnlockedSkills = mainPlayerBuildContext.UnlockedCharacterSkills,
            UnlockedProfessions = mainPlayerBuildContext.UnlockedProfessions,
        };
    }
}
