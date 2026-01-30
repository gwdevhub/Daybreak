using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
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
    PreferencesService preferencesService,
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
    private readonly PreferencesService preferencesService = preferencesService.ThrowIfNull();
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
                foreach (var charContext in *availableCharsContext.Pointer)
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

                // TODO: Reforged sometimes returns Zero UUID even when in-game, need to investigate why
                if (currentUuid != Uuid.Zero.ToString())
                {
                    var currentCharacter = availableChars.FirstOrDefault(c => c.Uuid == currentUuid);
                    return new CharacterSelectInformation(currentCharacter, availableChars);
                }
                else
                {
                    var currentCharacter = availableChars.FirstOrDefault();
                    return new CharacterSelectInformation(currentCharacter, availableChars);
                }
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

        var selectResult = await this.WaitForCharSelectAndSelectCharacter(characterName, cancellationToken);
        if (!selectResult)
        {
            scopedLogger.LogError("Failed to select character {name}", characterName);
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

        var selectResult = await this.WaitForCharSelectAndSelectCharacter(desiredCharName, cancellationToken);
        if (!selectResult)
        {
            scopedLogger.LogError("Failed to select character {name} with uuid {uuid}", desiredCharName, uuid);
            return false;
        }

        return true;
    }

    private async Task<bool> SelectCharacterToPlay(string characterName, bool play, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return await this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                var selectorFrame = this.GetCharSelectorFrame();
                if (selectorFrame is null)
                {
                    scopedLogger.LogError("Character selector frame not found");
                    return false;
                }

                var ctx = this.uiContextService.GetFrameContext<CharSelectorContext>(selectorFrame);
                if (ctx.IsNull)
                {
                    scopedLogger.LogError("Character selector context not found");
                    return false;
                }

                var panesFrame = this.uiContextService.GetChildFrame(selectorFrame, 0);
                if (panesFrame.IsNull)
                {
                    scopedLogger.LogError("Character panes frame not found");
                    return false;
                }

                // Get current selected index
                var selectedIdx = 0;
                this.uiContextService.SendFrameUIMessage(panesFrame, UIMessage.FrameMessage_QuerySelectedIndex, null, &selectedIdx);

                var chosen = false;
                for (uint i = 0; !chosen && i < ctx.Pointer->Chars.Size; i++)
                {
                    var c = ctx.Pointer->Chars.Buffer[i];
                    var charNameSpan = c.Pointer->Name.AsSpan();
                    var nullIdx = charNameSpan.IndexOf('\0');
                    if (nullIdx <= 0)
                    {
                        continue;
                    }

                    var charName = new string(charNameSpan[..nullIdx]);
                    if (!charName.StartsWith(characterName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    while (selectedIdx != i)
                    {
                        var keyAction = new UIPackets.KeyAction(0x1c);
                        this.uiContextService.SendFrameUIMessage(panesFrame, UIMessage.KeyDown, &keyAction);

                        var newIdx = selectedIdx;
                        this.uiContextService.SendFrameUIMessage(panesFrame, UIMessage.FrameMessage_QuerySelectedIndex, null, &newIdx);

                        if (newIdx == selectedIdx)
                        {
                            break; // This shouldn't happen - the character should have changed
                        }

                        selectedIdx = newIdx;
                    }

                    chosen = selectedIdx == i;
                    break;
                }

                if (!chosen)
                {
                    scopedLogger.LogError("Failed to select character {name}", characterName);
                    return false;
                }

                // TODO: This needs to be reworked to use UI messages to click on Play
                var hwnd = this.platformContextService.GetWindowHandle();
                if (!hwnd.HasValue)
                {
                    scopedLogger.LogError("Failed to get game window handle");
                    return false;
                }

                NativeMethods.SendMessageW((nint)hwnd.Value, NativeMethods.WM_KEYDOWN, 0x50, 0x00190001);
                NativeMethods.SendMessageW((nint)hwnd.Value, NativeMethods.WM_CHAR, 0x70, 0x00190001);
                NativeMethods.SendMessageW((nint)hwnd.Value, NativeMethods.WM_KEYUP, 0x50, 0x00190001);
                return true;
            }
        }, cancellationToken);
    }

    private async Task<bool> WaitForCharSelectAndSelectCharacter(string characterName, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        // Wait for character select to be ready
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (await this.gameThreadService.QueueOnGameThread(this.IsCharSelectReady, cancellationToken))
                {
                    break;
                }

                await Task.Delay(100, cancellationToken);
            }
            catch(Exception e)
            {
                scopedLogger.LogError(e, "Error while waiting for character select to be ready");
                throw;
            }
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        var previousOrderPreference = await this.gameThreadService.QueueOnGameThread(() => this.preferencesService.GetEnumPreference(EnumPreference.CharSortOrder), cancellationToken);
        if (previousOrderPreference is null)
        {
            scopedLogger.LogError("Failed to get previous character sort order preference");
            return false;
        }

        scopedLogger.LogInformation("Previous character sort order preference: {order}. Setting order to alphabetize", (CharSortOrder)previousOrderPreference);
        await this.gameThreadService.QueueOnGameThread(() => this.preferencesService.SetEnumPreference(EnumPreference.CharSortOrder, (uint)CharSortOrder.Alphabetize), cancellationToken);
        try
        {
            return await this.SelectCharacterToPlay(characterName, play: true, cancellationToken);
        }
        finally
        {
            // TODO: Delay to ensure character select has fully processed the selection before reverting sort order. Should be done after loading into the map.
            await Task.Delay(1000, cancellationToken);
            await this.gameThreadService.QueueOnGameThread(() => this.preferencesService.SetEnumPreference(EnumPreference.CharSortOrder, previousOrderPreference.Value), cancellationToken);
        }
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
            var logoutMessage = new LogOutMessage(0, 1); // Changed to 1 for character select
            unsafe
            {
                this.uiContextService.SendMessage(UIMessage.Logout, (uint)&logoutMessage, 0);
            }
        }, cancellationToken);
    }

    private async Task<bool> ValidateState(CancellationToken cancellationToken)
    {
        return await this.gameThreadService.QueueOnGameThread(() =>
        {
            return this.instanceContextService.GetInstanceType() is not API.Interop.GuildWars.InstanceType.Loading;
        }, cancellationToken);
    }

    private unsafe bool IsCharSelectReady()
    {
        return this.GetCharSelectorFrame() is not null;
    }

    private unsafe Frame* GetCharSelectorFrame()
    {
        var selectorFrame = this.uiContextService.GetFrameByLabel("Selector");
        if (selectorFrame.IsNull)
        {
            return null;
        }

        return selectorFrame.Pointer;
    }
}
