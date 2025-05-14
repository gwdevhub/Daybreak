namespace Daybreak.Shared.Models.Interop;

public readonly struct HeroPartyMember
{
    public readonly uint AgentId;
    public readonly uint OwnerPlayerId;
    public readonly uint HeroId;
    private readonly uint H000C;
    private readonly uint H0010;
    public readonly uint Level;
}
