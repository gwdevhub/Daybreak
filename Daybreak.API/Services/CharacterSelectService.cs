using Daybreak.API.Interop;
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

    private async Task<bool> ValidateState(CancellationToken cancellationToken)
    {
        return await this.gameThreadService.QueueOnGameThread(() =>
        {
            return this.instanceContextService.GetInstanceType() is not API.Interop.GuildWars.InstanceType.Loading;
        }, cancellationToken);
    }

    /// <summary>
    /// Checks if the character select screen is ready by verifying the CharacterSelector frame has a valid context.
    /// Based on GWToolboxpp's LoginMgr::IsCharSelectReady().
    /// </summary>
    private unsafe bool IsCharSelectReady()
    {
        var selectorFrame = this.uiContextService.GetFrameByLabel("CharacterSelector");
        if (selectorFrame.IsNull)
        {
            return false;
        }

        return selectorFrame.Pointer->FrameContext != null;
    }

    /// <summary>
    /// Gets the CharSelectorContext from the CharacterSelector frame.
    /// </summary>
    private unsafe CharSelectorContext* GetCharSelectorContext()
    {
        var selectorFrame = this.uiContextService.GetFrameByLabel("CharacterSelector");
        if (selectorFrame.IsNull)
        {
            return null;
        }

        return (CharSelectorContext*)selectorFrame.Pointer->FrameContext;
    }

    /// <summary>
    /// Selects a character by name using frame-based UI messages.
    /// Based on GWToolboxpp's LoginMgr::SelectCharacterToPlay().
    /// </summary>
    private async Task<bool> SelectCharacterToPlay(string characterName, bool play, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        return await this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                var selectorFrame = this.uiContextService.GetFrameByLabel("CharacterSelector");
                if (selectorFrame.IsNull)
                {
                    scopedLogger.LogError("CharacterSelector frame not found");
                    return false;
                }

                var ctx = (CharSelectorContext*)selectorFrame.Pointer->FrameContext;
                if (ctx == null)
                {
                    scopedLogger.LogError("CharacterSelector context is null");
                    return false;
                }

                var panesFrame = this.uiContextService.GetChildFrame(selectorFrame, 0);
                if (panesFrame.IsNull)
                {
                    scopedLogger.LogError("Character panes frame not found");
                    return false;
                }

                // Get current selected index
                uint selectedIdx = 0;
                this.uiContextService.SendFrameUIMessage(panesFrame, UIMessage.FrameMessage_QuerySelectedIndex, null, &selectedIdx);

                // Find the target character index
                uint? targetIndex = null;
                for (uint i = 0; i < ctx->Chars.Size; i++)
                {
                    var charNameSpan = ctx->Chars.Buffer[i].Name.AsSpan();
                    var nullIdx = charNameSpan.IndexOf('\0');
                    if (nullIdx <= 0)
                    {
                        continue; // Skip empty slots
                    }

                    var charName = new string(charNameSpan[..nullIdx]);
                    if (charName == characterName)
                    {
                        targetIndex = i;
                        break;
                    }
                }

                if (targetIndex is null)
                {
                    scopedLogger.LogError("Character {name} not found in selector", characterName);
                    return false;
                }

                // Navigate to the target character using arrow key emulation
                var keyAction = new UIPackets.KeyAction(0x1c); // Right arrow key in GW UI terms
                while (selectedIdx != targetIndex.Value)
                {
                    this.uiContextService.SendFrameUIMessage(panesFrame, UIMessage.KeyDown, &keyAction);

                    uint newIdx = selectedIdx;
                    this.uiContextService.SendFrameUIMessage(panesFrame, UIMessage.FrameMessage_QuerySelectedIndex, null, &newIdx);

                    if (newIdx == selectedIdx)
                    {
                        scopedLogger.LogError("Failed to navigate to character - index didn't change");
                        return false;
                    }

                    selectedIdx = newIdx;
                }

                if (selectedIdx != targetIndex.Value)
                {
                    scopedLogger.LogError("Failed to navigate to character {name}", characterName);
                    return false;
                }

                if (play)
                {
                    // Click the Play button
                    var playButton = this.uiContextService.GetFrameByLabel("Play");
                    if (playButton.IsNull)
                    {
                        scopedLogger.LogError("Play button not found");
                        return false;
                    }

                    this.uiContextService.SendFrameUIMessage(playButton, Models.UIMessage.MouseClick, null);
                }

                return true;
            }
        }, cancellationToken);
    }

    /// <summary>
    /// Waits for the character select screen to be ready and then selects the specified character.
    /// </summary>
    private async Task<bool> WaitForCharSelectAndSelectCharacter(string characterName, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        // Wait for character select to be ready
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (await this.gameThreadService.QueueOnGameThread(() =>
                {
                    return this.IsCharSelectReady();
                }, cancellationToken))
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

        scopedLogger.LogInformation("Character select ready, selecting {name}", characterName);

        // Select the character and click play
        return await this.SelectCharacterToPlay(characterName, play: true, cancellationToken);
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
                this.uiContextService.SendMessage(Models.UIMessage.Logout, (uint)&logoutMessage, 0);
            }
        }, cancellationToken);
    }
}
