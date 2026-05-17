using Daybreak.API.Extensions;
using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models.Api;
using System.Core.Extensions;
using System.Extensions;
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
                    gameContext.Pointer->World is null)
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

                var currentUuid = (*(Uuid*)gameContext.Pointer->Character->PlayerUuid).ToString();
                var availableChars = new List<CharacterSelectEntry>((int)availableCharsContext.Pointer->Size);
                foreach (var charContext in *availableCharsContext.Pointer)
                {
                    var name = new string(charContext.Name);
                    availableChars.Add(new CharacterSelectEntry(
                        (*(Uuid*)charContext.Uuid).ToString(),
                        name,
                        (uint)charContext.GetPrimaryProfession(),
                        (uint)charContext.GetSecondaryProfession(),
                        (uint)charContext.GetCampaign(),
                        (uint)charContext.GetMapId(),
                        (uint)charContext.GetLevel(),
                        charContext.IsPvP()));
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private unsafe struct CharSelectButtonParam
    {
        public char* Name;
        public uint Play;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    private readonly unsafe struct CharSelectorContext
    {
        [FieldOffset(0x0000)]
        public readonly uint Vtable;

        [FieldOffset(0x0004)]
        public readonly uint FrameId;

        [FieldOffset(0x0008)]
        public readonly GuildWarsArray<WrappedPointer<CharSelectorChar>> Chars;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    private readonly struct CharSelectorChar
    {
        [FieldOffset(0x0020)]
        public readonly Array20Char Name;
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
                uint selectedIdx = 0;
                this.uiContextService.SendFrameUIMessage(panesFrame, UIMessage.kFrameMessage_0x4a, null, &selectedIdx);

                // Find target character index
                uint targetIdx = 0xFFFF;
                var charCount = ctx.Pointer->Chars.Size;

                for (uint i = 0; i < charCount; i++)
                {
                    var c = ctx.Pointer->Chars.Buffer[i];
                    if (c.IsNull)
                    {
                        continue;
                    }

                    var charNameSpan = c.Pointer->Name.AsSpan();
                    var nullIdx = charNameSpan.IndexOf('\0');
                    if (nullIdx <= 0)
                    {
                        continue;
                    }

                    var charName = new string(charNameSpan[..nullIdx]);
                    if (!charName.Equals(characterName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    targetIdx = i;
                    break;
                }

                if (targetIdx >= charCount)
                {
                    scopedLogger.LogError("Character {name} not found in character list", characterName);
                    return false;
                }

                var parentFrame = this.uiContextService.GetParentFrame(selectorFrame);
                if (parentFrame.IsNull)
                {
                    scopedLogger.LogError("Parent frame not found");
                    return false;
                }

                // Helper function to select a character at a specific index
                bool SelectChar(uint idx)
                {
                    var charToSelect = ctx.Pointer->Chars.Buffer[idx];
                    if (charToSelect.IsNull)
                    {
                        return false;
                    }

                    fixed (char* namePtr = charToSelect.Pointer->Name.AsSpan())
                    {
                        var buttonParam = new CharSelectButtonParam
                        {
                            Name = namePtr,
                            Play = 0
                        };

                        var action = new kMouseAction
                        {
                            FrameId = selectorFrame->FrameId,
                            ChildOffsetId = selectorFrame->ChildOffsetId,
                            CurrentState = GWCA.GW.UI.UIPacket.ActionState.MouseClick,
                            Wparam = (nint)(&buttonParam)
                        };

                        if (!this.uiContextService.SendFrameUIMessage(parentFrame, UIMessage.kMouseClick2, &action))
                        {
                            return false;
                        }

                        uint newSelectedIdx = 0;
                        this.uiContextService.SendFrameUIMessage(panesFrame, UIMessage.kFrameMessage_0x4a, null, &newSelectedIdx);
                        return newSelectedIdx == idx;
                    }
                }

                // Navigate to target character by selecting previous/next until we reach it
                while (targetIdx < selectedIdx)
                {
                    if (selectedIdx == 0)
                    {
                        break;
                    }

                    if (!SelectChar(selectedIdx - 1))
                    {
                        scopedLogger.LogError("Failed to select previous character");
                        return false;
                    }

                    selectedIdx--;
                }

                while (targetIdx > selectedIdx)
                {
                    if (selectedIdx >= charCount - 1)
                    {
                        break;
                    }

                    if (!SelectChar(selectedIdx + 1))
                    {
                        scopedLogger.LogError("Failed to select next character");
                        return false;
                    }

                    selectedIdx++;
                }

                var chosen = selectedIdx == targetIdx;
                if (!chosen)
                {
                    scopedLogger.LogError("Failed to select character {name}", characterName);
                    return false;
                }

                if (!play)
                {
                    return true;
                }

                var playButton = this.uiContextService.GetFrameByLabel("Play");
                if (playButton.IsNull)
                {
                    scopedLogger.LogError("Play button not found");
                    return false;
                }

                return this.uiContextService.ButtonClick(playButton);
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
            catch (Exception e)
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
                    if ((*(Uuid*)charContext.Uuid).ToString() != uuid)
                    {
                        continue;
                    }

                    var name = new string(charContext.Name);
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
            var logoutMessage = new kLogout { Unknown = 0, CharacterSelect = 1 };
            unsafe
            {
                this.uiContextService.SendMessage(UIMessage.kLogout, (uint)&logoutMessage, 0);
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
