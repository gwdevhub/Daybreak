using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using System.Diagnostics.CodeAnalysis;

namespace Daybreak.API.Extensions;

public static unsafe class GameContextExtensions
{
    public static bool TryGetPlayerId(
        this WrappedPointer<GameContext> gameContext,
        [NotNullWhen(true)] out uint? playerId)
    {
        playerId = 0;
        if (gameContext.IsNull ||
            gameContext.Pointer->World is null ||
            gameContext.Pointer->World->PlayerControlledChar is null)
        {
            return false;
        }

        playerId = gameContext.Pointer->World->PlayerControlledChar->AgentId;
        return true;
    }

    public static bool TryGetBuildContext(
        this WrappedPointer<GameContext> gameContext,
        [NotNullWhen(true)] out GuildWarsArray<SkillbarData>? skillbars,
        [NotNullWhen(true)] out GuildWarsArray<PartyAttribute>? attributes,
        [NotNullWhen(true)] out GuildWarsArray<ProfessionState>? professions,
        [NotNullWhen(true)] out GuildWarsArray<uint>? unlockedSkills)
    {
        skillbars = default;
        attributes = default;
        professions = default;
        unlockedSkills = default;
        if (gameContext.IsNull ||
            gameContext.Pointer->World is null)
        {
            return false;
        }

        skillbars = gameContext.Pointer->World->Skillbar.Value;
        attributes = gameContext.Pointer->World->Attributes.Value;
        professions = gameContext.Pointer->World->PartyProfessionStates;
        unlockedSkills = gameContext.Pointer->World->UnlockedCharacterSkills;
        return true;
    }

    public static bool TryGetPlayerParty(
        this WrappedPointer<GameContext> gameContext,
        [NotNullWhen(true)] out uint? partyId,
        [NotNullWhen(true)] out GuildWarsArray<PlayerPartyMember>? players,
        [NotNullWhen(true)] out GuildWarsArray<HeroPartyMember>? heroes,
        [NotNullWhen(true)] out GuildWarsArray<HenchmanPartyMember>? henchmen)
    {
        partyId = default;
        players = default;
        heroes = default;
        henchmen = default;
        if (gameContext.IsNull ||
            gameContext.Pointer->Party is 0)
        {
            return false;
        }

        var partyInfo = GWCA.GW.PartyMgr.GetPartyInfo(0);
        partyId = partyInfo->PartyId;
        players = partyInfo->Players;
        heroes = partyInfo->Heroes;
        henchmen = partyInfo->Henchmen;
        return true;
    }

    public static bool TryGetHeroFlags(
        this WrappedPointer<GameContext> gameContext,
        [NotNullWhen(true)]out GuildWarsArray<HeroFlag>? heroFlags)
    {
        heroFlags = default;
        if (gameContext.IsNull ||
            gameContext.Pointer->World is null)
        {
            return false;
        }

        heroFlags = gameContext.Pointer->World->HeroFlags.Value;
        return true;
    }

    public static bool TryGetAccountContext(
        this WrappedPointer<GameContext> gameContext,
        out WrappedPointer<AccountContext> accountContext)
    {
        accountContext = default;
        if (gameContext.IsNull ||
            gameContext.Pointer->Account is null)
        {
            return false;
        }

        accountContext = gameContext.Pointer->Account;
        return true;
    }
}
