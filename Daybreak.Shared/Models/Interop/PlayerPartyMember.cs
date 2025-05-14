namespace Daybreak.Shared.Models.Interop;

public readonly struct PlayerPartyMember
{
    public readonly uint LoginNumber;
    public readonly uint CalledTargetId;
    public readonly uint State;

    public bool Connected => (this.State & 1) > 0;
    public bool Ticked => (this.State & 2) > 0;
}
