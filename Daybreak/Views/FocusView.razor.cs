using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.FocusView;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;

//TODO: Setup FocusView
public sealed class FocusViewModel(
    IViewManager viewManager,
    INotificationService notificationService,
    ILaunchConfigurationService launchConfigurationService,
    IDaybreakApiService daybreakApiService,
    ILogger<FocusView> logger)
    : ViewModelBase<FocusViewModel, FocusView>
{
    private const int MaxRetryAttempts = 5;

    private static readonly TimeSpan GameInfoFrequency = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan GameInfoTimeout = TimeSpan.FromSeconds(5);

    private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILaunchConfigurationService launchConfigurationService = launchConfigurationService.ThrowIfNull();
    private readonly IDaybreakApiService daybreakApiService = daybreakApiService.ThrowIfNull();
    private readonly ILogger<FocusView> logger = logger.ThrowIfNull();

    private ScopedApiContext? apiContext;
    private Process? process;
    private CancellationTokenSource? cancellationSource = default;

    public CharacterComponentContext? CharacterComponentContext { get; private set; }
    public CurrentMapComponentContext? CurrentMapComponentContext { get; private set; }
    public PlayerResourcesComponentContext? PlayerResourcesComponentContext { get; private set; }
    public QuestLogComponentContext? QuestLogComponentContext { get; private set; }
    public TitleInformationComponentContext? TitleInformationComponentContext { get; private set; }
    public VanquishComponentContext? VanquishComponentContext { get; private set; }
    public BuildComponentContext? BuildComponentContext { get; private set; }

    public override async ValueTask ParametersSet(FocusView view, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var launchConfigId = view.ConfigurationId;
        var daybreakLaunchConfig = this.launchConfigurationService.GetLaunchConfigurations()
            .FirstOrDefault(x => x.Identifier == launchConfigId);
        if (daybreakLaunchConfig is null)
        {
            scopedLogger.LogError("Launch configuration with ID {configId} not found", launchConfigId);
            this.notificationService.NotifyError(
                title: "Focus View Error",
                description: $"Could not find launch configuration by id {launchConfigId}");

            this.viewManager.ShowView<LaunchView>();
            return;
        }

        if (!int.TryParse(view.ProcessId, out var processId))
        {
            scopedLogger.LogError("Process id is invalid {processId}", view.ProcessId);
            this.notificationService.NotifyError(
                title: "Focus View Error",
                description: $"Could not find GuildWars process by id {processId}");

            this.viewManager.ShowView<LaunchView>();
            return;
        }

        this.process = Process.GetProcessById(processId);
        var apiContext = await this.daybreakApiService.GetDaybreakApiContext(this.process, cancellationToken);
        if (apiContext is null)
        {
            scopedLogger.LogError("Could not attach to GuildWars process");
            this.notificationService.NotifyError(
                title: "Focus View Error",
                description: $"Could not attach to GuildWars process");

            this.viewManager.ShowView<LaunchView>();
            return;
        }

        this.apiContext = apiContext;
        this.Initialize();
    }

    private void ViewManager_ShowViewRequested(object? _, TrailBlazr.Models.ViewRequest e)
    {
        if (e.ViewModelType == typeof(FocusViewModel))
        {
            return;
        }

        this.Cleanup();
    }

    private void Initialize()
    {
        this.viewManager.ShowViewRequested += this.ViewManager_ShowViewRequested;
        this.cancellationSource?.Cancel();
        this.cancellationSource?.Dispose();
        this.cancellationSource = new CancellationTokenSource();
        var ct = this.cancellationSource.Token;
        this.SetupDefaultValues();
        Task.Factory.StartNew(() => this.PeriodicallyFetchGameInformation(ct), ct, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    private void Cleanup()
    {
        this.viewManager.ShowViewRequested -= this.ViewManager_ShowViewRequested;
        this.cancellationSource?.Cancel();
        this.cancellationSource?.Dispose();
        this.cancellationSource = default;
    }

    private void SetupDefaultValues()
    {
        this.CharacterComponentContext = default;
        this.CurrentMapComponentContext = default;
        this.PlayerResourcesComponentContext = default;
        this.QuestLogComponentContext = default;
        this.TitleInformationComponentContext = default;
        this.VanquishComponentContext = default;
        this.BuildComponentContext = default;
        this.RefreshView();
    }

    private async Task PeriodicallyFetchGameInformation(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var retryCount = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                scopedLogger.LogInformation("Cancellation requested, stopping game information fetch loop");
                return;
            }

            if (this.apiContext is null || this.process is null || this.process.HasExited)
            {
                scopedLogger.LogInformation("Process has exited or API context is null, stopping game information fetch loop");
                return;
            }

            if (!await this.FetchGameInformation(cancellationToken))
            {
                retryCount++;
            }
            else
            {
                retryCount = 0;
                await this.RefreshViewAsync();
            }

            if (retryCount >= MaxRetryAttempts)
            {
                return;
            }

            await Task.Delay(GameInfoFrequency, cancellationToken);
        }
    }

    private async ValueTask<bool> FetchGameInformation(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.apiContext is null)
        {
            return false;
        }

        var timeoutCts = new CancellationTokenSource(GameInfoTimeout);
        var timeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
        var timeoutToken = timeoutTokenSource.Token;

        var characterListTask = this.apiContext.GetCharacters(timeoutToken);
        var mainPlayerInstanceTask = this.apiContext.GetMainPlayerInstanceInfo(timeoutToken);
        var mainPlayerStateTask = this.apiContext.GetMainPlayerState(timeoutToken);
        var titleInfoTask = this.apiContext.GetTitleInfo(timeoutToken);
        var questLogTask = this.apiContext.GetMainPlayerQuestLog(timeoutToken);
        var mainPlayerBuildContextTask = this.apiContext.GetMainPlayerBuildContext(timeoutToken);
        try
        {
            await Task.WhenAny([
                characterListTask,
                mainPlayerInstanceTask,
                mainPlayerStateTask,
                titleInfoTask,
                questLogTask,
                mainPlayerBuildContextTask
                ]);
        }
        catch (TaskCanceledException)
        {
            scopedLogger.LogWarning("Timeout occurred while fetching game information");
            return false;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Error fetching game information");
            return false;
        }

        var characterList = await characterListTask;
        var mainPlayerInstance = await mainPlayerInstanceTask;
        var mainPlayerState = await mainPlayerStateTask;
        var titleInformation = await titleInfoTask;
        var questLog = await questLogTask;
        var mainPlayerBuildContext = await mainPlayerBuildContextTask;

        this.CharacterComponentContext = ParseCharacterComponentContext(characterList, mainPlayerState);
        this.CurrentMapComponentContext = ParseCurrentMapComponentContext(mainPlayerInstance);
        this.PlayerResourcesComponentContext = ParsePlayerResourcesContext(mainPlayerState);
        this.QuestLogComponentContext = ParseQuestLogComponentContext(questLog);
        this.TitleInformationComponentContext = ParseTitleInformationComponentContext(titleInformation);
        this.VanquishComponentContext = ParseVanquishComponentContext(mainPlayerInstance);
        this.BuildComponentContext = ParseBuildComponentContext(mainPlayerInstance, mainPlayerBuildContext);
        return true;
    }

    private static CharacterComponentContext? ParseCharacterComponentContext(CharacterSelectInformation? characterSelectInformation, MainPlayerState? mainPlayerState)
    {
        if (characterSelectInformation is null ||
            characterSelectInformation.CharacterNames is null ||
            characterSelectInformation.CurrentCharacter is null)
        {
            return default;
        }

        var parsedCharacters = characterSelectInformation.CharacterNames.Select(entry =>
        {
            if (!Profession.TryParse((int)entry.Primary, out var primaryProfession))
            {
                primaryProfession = Profession.None;
            }

            if (!Profession.TryParse((int)entry.Secondary, out var secondaryProfession))
            {
                secondaryProfession = Profession.None;
            }

            return new CharacterSelectComponentEntry
            {
                CharacterName = entry.Name,
                DisplayName = $"{primaryProfession.Alias}{(secondaryProfession != Profession.None && secondaryProfession is not null ? $"/{secondaryProfession.Alias}" : "")} {entry.Name}"
            };
        }).ToList();

        var selectedCharacter = parsedCharacters.FirstOrDefault(x => x.CharacterName == characterSelectInformation.CurrentCharacter.Name);
        if (selectedCharacter is null)
        {
            return default;
        }

        return new CharacterComponentContext { Characters = parsedCharacters, CurrentCharacter = selectedCharacter, CurrentExperience = mainPlayerState?.CurrentExperience ?? 0 };
    }

    private static CurrentMapComponentContext? ParseCurrentMapComponentContext(InstanceInfo? mainPlayerInstance)
    {
        _ = Map.TryParse(((int?)mainPlayerInstance?.MapId) ?? -1, out var currentMap);
        if (currentMap is null)
        {
            return default;
        }

        return new CurrentMapComponentContext { CurrentMap = currentMap };
    }

    private static PlayerResourcesComponentContext? ParsePlayerResourcesContext(MainPlayerState? mainPlayerState)
    {
        if (mainPlayerState is null)
        {
            return default;
        }

        return new PlayerResourcesComponentContext
        {
            CurrentBalthazar = mainPlayerState.CurrentBalthazar,
            MaxBalthazar = mainPlayerState.MaxBalthazar,
            TotalBalthazar = mainPlayerState.TotalBalthazar,
            CurrentImperial = mainPlayerState.CurrentImperial,
            MaxImperial = mainPlayerState.MaxImperial,
            TotalImperial = mainPlayerState.TotalImperial,
            CurrentKurzick = mainPlayerState.CurrentKurzick,
            MaxKurzick = mainPlayerState.MaxKurzick,
            TotalKurzick = mainPlayerState.TotalKurzick,
            CurrentLuxon = mainPlayerState.CurrentLuxon,
            MaxLuxon = mainPlayerState.MaxLuxon,
            TotalLuxon = mainPlayerState.TotalLuxon
        };
    }

    private static QuestLogComponentContext? ParseQuestLogComponentContext(QuestLogInformation? questLog)
    {
        if (questLog is null)
        {
            return default;
        }

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

        return new QuestLogComponentContext
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

    private static TitleInformationComponentContext? ParseTitleInformationComponentContext(TitleInfo? titleInfo)
    {
        if (titleInfo is null ||
            !Title.TryParse((int)titleInfo.Id, out var title))
        {
            return default;
        }

        return new TitleInformationComponentContext
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

    private static VanquishComponentContext? ParseVanquishComponentContext(InstanceInfo? instanceInfo)
    {
        if (instanceInfo is null)
        {
            return default;
        }

        return new VanquishComponentContext
        {
            FoesKilled = instanceInfo.FoesKilled,
            FoesToKill = instanceInfo.FoesToKill,
            HardMode = instanceInfo.Difficulty is DifficultyInfo.Hard,
            Vanquishing = (instanceInfo.FoesToKill + instanceInfo.FoesKilled > 0U) && instanceInfo.Difficulty is DifficultyInfo.Hard
        };
    }

    private static BuildComponentContext? ParseBuildComponentContext(InstanceInfo? instanceInfo, MainPlayerBuildContext? mainPlayerBuildContext)
    {
        if (instanceInfo is null || mainPlayerBuildContext is null)
        {
            return default;
        }

        return new BuildComponentContext
        {
            IsInOutpost = instanceInfo.Type is Shared.Models.Api.InstanceType.Outpost,
            PrimaryProfessionId = mainPlayerBuildContext.PrimaryProfessionId,
            AccountUnlockedSkills = mainPlayerBuildContext.UnlockedAccountSkills,
            CharacterUnlockedSkills = mainPlayerBuildContext.UnlockedCharacterSkills,
            UnlockedProfessions = mainPlayerBuildContext.UnlockedProfessions,
        };
    }
}
