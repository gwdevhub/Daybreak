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
            gameContext.Pointer->WorldContext is null ||
            gameContext.Pointer->WorldContext->PlayerControlledChar is null)
        {
            return false;
        }

        playerId = gameContext.Pointer->WorldContext->PlayerControlledChar->AgentId;
        return true;
    }

    public static bool TryGetBuildContext(
        this WrappedPointer<GameContext> gameContext,
        [NotNullWhen(true)] out GuildWarsArray<SkillbarContext>? skillbars,
        [NotNullWhen(true)] out GuildWarsArray<PartyAttribute>? attributes,
        [NotNullWhen(true)] out GuildWarsArray<ProfessionsContext>? professions,
        [NotNullWhen(true)] out GuildWarsArray<uint>? unlockedSkills)
    {
        skillbars = default;
        attributes = default;
        professions = default;
        unlockedSkills = default;
        if (gameContext.IsNull ||
            gameContext.Pointer->WorldContext is null)
        {
            return false;
        }

        skillbars = gameContext.Pointer->WorldContext->Skillbars;
        attributes = gameContext.Pointer->WorldContext->Attributes;
        professions = gameContext.Pointer->WorldContext->Professions;
        unlockedSkills = gameContext.Pointer->WorldContext->UnlockedCharacterSkills;
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
            gameContext.Pointer->PartyContext is null)
        {
            return false;
        }

        partyId = gameContext.Pointer->PartyContext->PlayerParty->PartyId;
        players = gameContext.Pointer->PartyContext->PlayerParty->Players;
        heroes = gameContext.Pointer->PartyContext->PlayerParty->Heroes;
        henchmen = gameContext.Pointer->PartyContext->PlayerParty->Henchmen;
        return true;
    }

    public static bool TryGetHeroFlags(
        this WrappedPointer<GameContext> gameContext,
        [NotNullWhen(true)]out GuildWarsArray<HeroFlag>? heroFlags)
    {
        heroFlags = default;
        if (gameContext.IsNull ||
            gameContext.Pointer->WorldContext is null)
        {
            return false;
        }

        heroFlags = gameContext.Pointer->WorldContext->HeroFlags;
        return true;
    }

    public static bool TryGetAccountContext(
        this WrappedPointer<GameContext> gameContext,
        out WrappedPointer<AccountGameContext> accountContext)
    {
        accountContext = default;
        if (gameContext.IsNull ||
            gameContext.Pointer->AccountContext is null)
        {
            return false;
        }

        accountContext = gameContext.Pointer->AccountContext;
        return true;
    }
}
