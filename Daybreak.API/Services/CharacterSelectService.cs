using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models.Api;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using ZLinq;

namespace Daybreak.API.Services;

public sealed class CharacterSelectService(
    InstanceContextService instanceContextService,
    PlatformContextService platformContextService,
    UIContextService uiContextService,
    GameThreadService gameThreadService,
    GameContextService gameContextService,
    ILogger<CharacterSelectService> logger)
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private readonly struct LogOutMessage(uint unknown, uint characterSelect)
    {
        public readonly uint Unknown = unknown;
        public readonly uint CharacterSelect = characterSelect;
    }

    private readonly InstanceContextService instanceContextService = instanceContextService.ThrowIfNull();
    private readonly PlatformContextService platformContextService = platformContextService.ThrowIfNull();
    private readonly UIContextService uiContextService = uiContextService.ThrowIfNull();
    private readonly GameThreadService gameThreadService = gameThreadService.ThrowIfNull();
    private readonly GameContextService gameContextService = gameContextService.ThrowIfNull();
    private readonly ILogger<CharacterSelectService> logger = logger.ThrowIfNull();

    public Task<CharacterSelectInformation?> GetCharacterSelectInformation(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull ||
                    gameContext.Pointer->WorldContext is null)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return default;
                }

                var availableCharsContext = this.gameContextService.GetAvailableChars();
                if (availableCharsContext.IsNull)
                {
                    scopedLogger.LogError("Available characters context is not initialized");
                    return default;
                }

                var currentUuid = gameContext.Pointer->CharContext->PlayerUuid.ToString();
                var availableChars = new List<CharacterSelectEntry>((int)availableCharsContext.Pointer->Size);
                foreach(var charContext in *availableCharsContext.Pointer)
                {
                    var nameSpan = charContext.Name.AsSpan();
                    var name = new string(nameSpan[..nameSpan.IndexOf('\0')]);
                    availableChars.Add(new CharacterSelectEntry(
                        charContext.Uuid.ToString(),
                        name,
                        charContext.Primary,
                        charContext.Secondary,
                        charContext.Campaign,
                        charContext.MapId,
                        charContext.Level,
                        charContext.IsPvp));
                }

                var currentCharacter = availableChars.FirstOrDefault(c => c.Uuid == currentUuid);
                return new CharacterSelectInformation(currentCharacter, availableChars);
            }
        }, cancellationToken);
    }

    public async Task<bool> ChangeCharacterByName(string characterName, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (await this.ValidateState(cancellationToken) is false)
        {
            scopedLogger.LogError("Cannot change character while in loading state");
            return false;
        }

        await this.TriggerLogOut(cancellationToken);

        var indexResult = await this.WaitForLoginScreenAndGetCurrentAndDesiredIndex(characterName, cancellationToken);
        if (indexResult is null)
        {
            scopedLogger.LogError("Failed to find index by name {name}", characterName);
            return false;
        }

        var currentIndex = indexResult.Value.CurrentIndex;
        var desiredIndex = indexResult.Value.DesiredIndex;
        scopedLogger.LogInformation("Changing character to {name} with index {index}", characterName, desiredIndex);
        var navigateResult = await this.NavigateToCharAndPlay(currentIndex, desiredIndex, cancellationToken);
        if (!navigateResult)
        {
            scopedLogger.LogError("Failed to navigate to character {name} with index {index}", characterName, desiredIndex);
            return false;
        }

        return true;
    }

    public async Task<bool> ChangeCharacterByUuid(string uuid, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (await this.ValidateState(cancellationToken) is false)
        {
            scopedLogger.LogError("Cannot change character while in loading state");
            return false;
        }

        var desiredCharName = await this.GetCharNameByUuid(uuid, cancellationToken);
        if (desiredCharName is null)
        {
            scopedLogger.LogError("Failed to find character with UUID: {uuid}", uuid);
            return false;
        }

        await this.TriggerLogOut(cancellationToken);

        var indexResult = await this.WaitForLoginScreenAndGetCurrentAndDesiredIndex(desiredCharName, cancellationToken);
        if (indexResult is null)
        {
            scopedLogger.LogError("Resolved character by uuid {uuid} but failed to find index by name {name}", uuid, desiredCharName);
            return false;
        }

        var currentIndex = indexResult.Value.CurrentIndex;
        var desiredIndex = indexResult.Value.DesiredIndex;
        scopedLogger.LogInformation("Changing character to {name} with index {index} and uuid {uuid}", desiredCharName, desiredIndex, uuid);
        var navigateResult = await this.NavigateToCharAndPlay(currentIndex, desiredIndex, cancellationToken);
        if (!navigateResult)
        {
            scopedLogger.LogError("Failed to navigate to character {name} with index {index} and uuid {uuid}", desiredCharName, desiredIndex, uuid);
            return false;
        }

        return true;
    }

    private async Task<bool> ValidateState(CancellationToken cancellationToken)
    {
        return await this.gameThreadService.QueueOnGameThread(() =>
        {
            return this.instanceContextService.GetInstanceType() is not API.Interop.GuildWars.InstanceType.Loading;
        }, cancellationToken);
    }

    private async Task<bool> NavigateToCharAndPlay(uint currentIndex, uint desiredIndex, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(100, cancellationToken);
            var result = await this.gameThreadService.QueueOnGameThread(() =>
            {
                unsafe
                {
                    var preGameContext = this.gameContextService.GetPreGameContext();
                    if (preGameContext.IsNull)
                    {
                        scopedLogger.LogError("Pre-game context is not initialized");
                        return false;
                    }

                    var hwnd = this.platformContextService.GetWindowHandle();
                    if (hwnd is null or 0)
                    {
                        scopedLogger.LogError("Failed to get window handle");
                        return false;
                    }

                    if (preGameContext.Pointer->Index1 == currentIndex)
                    {
                        return false; //Not moved yet
                    }

                    if (preGameContext.Pointer->Index1 == desiredIndex)
                    {
                        // We're on the desired character. Trigger play
                        NativeMethods.SendMessageW((nint)hwnd.Value, NativeMethods.WM_KEYDOWN, 0x50, 0x00190001);
                        NativeMethods.SendMessageW((nint)hwnd.Value, NativeMethods.WM_CHAR, 0x70, 0x00190001);
                        NativeMethods.SendMessageW((nint)hwnd.Value, NativeMethods.WM_KEYUP, 0x50, 0x00190001);
                        return true;
                    }

                    currentIndex = preGameContext.Pointer->Index1;
                    NativeMethods.SendMessageW((nint)hwnd.Value, NativeMethods.WM_KEYDOWN, NativeMethods.VK_RIGHT, 0x014D0001);
                    NativeMethods.SendMessageW((nint)hwnd.Value, NativeMethods.WM_KEYUP, NativeMethods.VK_RIGHT, 0x014D0001);
                    return false;
                }
            }, cancellationToken);

            if (result)
            {
                return true;
            }
        }

        return false;
    }

    private async Task<string?> GetCharNameByUuid(string uuid, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return await this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return default;
                }

                var availableCharsContext = this.gameContextService.GetAvailableChars();
                if (availableCharsContext.IsNull)
                {
                    scopedLogger.LogError("Available characters context is not initialized");
                    return default;
                }

                foreach (var charContext in *availableCharsContext.Pointer)
                {
                    if (charContext.Uuid.ToString() != uuid)
                    {
                        continue;
                    }

                    var nameSpan = charContext.Name.AsSpan();
                    var name = new string(nameSpan[..nameSpan.IndexOf('\0')]);
                    return name;
                }

                return default;
            }
        }, cancellationToken);
    }

    private async Task TriggerLogOut(CancellationToken cancellationToken)
    {
        await this.gameThreadService.QueueOnGameThread(() =>
        {
            var logoutMessage = new LogOutMessage(0, 0);
            unsafe
            {
                this.uiContextService.SendMessage(Models.UIMessage.Logout, (uint)&logoutMessage, 0);
            }
        }, cancellationToken);
    }

    private async Task<(uint DesiredIndex, uint CurrentIndex)?> WaitForLoginScreenAndGetCurrentAndDesiredIndex(string desiredCharName, CancellationToken cancellationToken)
    {
        uint? desiredIndex = default;
        uint? currentIndex = default;
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(100, cancellationToken);
            var ready = await this.gameThreadService.QueueOnGameThread(() =>
            {
                unsafe
                {
                    var preGameContext = this.gameContextService.GetPreGameContext();
                    if (preGameContext.IsNull)
                    {
                        return false;
                    }

                    var uiState = 10U;
                    this.uiContextService.SendMessage(Models.UIMessage.CheckUIState, 0, (uint)&uiState);
                    var loginScreen = uiState == 2;
                    if (!loginScreen)
                    {
                        return false;
                    }

                    for (var i = 0U; i < preGameContext.Pointer->LoginCharacters.Size; i++)
                    {
                        var loginCharacter = preGameContext.Pointer->LoginCharacters.Buffer[i];
                        var charNameSpan = loginCharacter.CharacterName.AsSpan();
                        var charName = new string(charNameSpan[..charNameSpan.IndexOf('\0')]);
                        if (charName == desiredCharName)
                        {
                            desiredIndex = i;
                            currentIndex = 0xffffffdd;
                            return true;
                        }
                    }

                    return false;
                }
            }, cancellationToken);

            if (ready)
            {
                break;
            }
        }

        if (desiredIndex is not null && currentIndex is not null)
        {
            return (desiredIndex.Value, currentIndex.Value);
        }

        return default;
    }
}
