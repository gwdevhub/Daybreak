using System.Numerics;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[Flags]
public enum AgentType : uint
{
    Living = 0xDB,
    Gadget = 0x200,
    Item   = 0x400
}

[Flags]
public enum AgentModelType : ushort
{
    NPC    = 0x2000,
    Player = 0x3000
}

[Flags]
public enum LivingAgentState : uint
{
    None = 0,
    InCombatStance              = 0x000001,
    HasQuest                    = 0x000002,
    Dead                        = 0x000008,
    Female                      = 0x000200,
    HasBossGlow                 = 0x000400,
    HidingCape                  = 0x001000,
    CanBeViewedInPartyWindow    = 0x020000,
    SpawnedSpirit               = 0x040000,
    BeingObservedOrPlayer       = 0x400000
}

public enum LivingAgentAllegiance : byte
{
    Ally_NonAttackable = 0x1,
    Neutral = 0x2,
    Enemy = 0x3,
    Spirit_Pet = 0x4,
    Minion = 0x5,
    Npc_Minipet = 0x6
};


[Flags]
public enum LivingAgentEffects : uint
{
    None            = 0,
    Bleeding        = 0x0001,
    Conditioned     = 0x0002,
    Crippled1       = 0x0008, // Part of the Crippled check
    Crippled        = 0x000A, // Combined flag for crippled (0x0008 | 0x0002)
    Dead            = 0x0010,
    DeepWound       = 0x0020,
    Poisoned        = 0x0040,
    Enchanted       = 0x0080,
    DegenHexed      = 0x0400,
    Hexed           = 0x0800,
    WeaponSpelled   = 0x8000
}

public enum LivingAgentModelState : ushort
{
    // Idle states
    Idle1 = 64,
    Idle2 = 68,
    Idle3 = 100,

    // Casting states
    Casting1 = 65,
    Casting2 = 581,

    // Moving states
    Moving1 = 12,
    Moving2 = 76,
    Moving3 = 204,

    // Attacking states
    Attacking1 = 96,
    Attacking2 = 1088,
    Attacking3 = 1120,

    // Other states
    KnockedDown = 1104
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[GWCAEquivalent("AgentEffects")]
public readonly struct AgentEffects
{
    public readonly uint AgentId;
    public readonly GuildWarsArray<Buff> Buffs;
    public readonly GuildWarsArray<Effect> Effects;
}

[StructLayout(LayoutKind.Explicit, Pack = 1)]
[GWCAEquivalent("AgentContext")]
public readonly struct AgentGameContext
{
    [FieldOffset(0x01AC)]
    public readonly uint InstanceTimer;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 196)]
[GWCAEquivalent("Agent")]
public readonly struct AgentContext
{
    [FieldOffset(0x0014)]
    public readonly uint AgentInstanceTimer;

    [FieldOffset(0x002C)]
    public readonly uint AgentId;

    [FieldOffset(0x0074)]
    public readonly GamePos Pos;

    [FieldOffset(0x009C)]
    public readonly AgentType Type;

    [FieldOffset(0x00A0)]
    public readonly Vector2 Velocity;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 212)]
[GWCAEquivalent("AgentItem")]
public readonly struct AgentItemContext
{
    [FieldOffset(0x00C4)]
    public readonly uint OwnerId;

    [FieldOffset(0x00C8)]
    public readonly uint ItemId;

    [FieldOffset(0x00D0)]
    public readonly uint ExtraType;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 228)]
[GWCAEquivalent("AgentGadget")]
public readonly struct AgentGadgetContext
{
    [FieldOffset(0x00CC)]
    public readonly uint ExtraType;

    [FieldOffset(0x00D0)]
    public readonly uint GadgetId;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x1C0)]
[GWCAEquivalent("AgentLiving")]
public readonly unsafe struct AgentLivingContext
{
    [FieldOffset(0x00C4)]
    public readonly uint OwnerId;

    [FieldOffset(0x00F4)]
    public readonly ushort AgentType; // All non-player have an identifier. Two of the same mob have the same identifier;

    [FieldOffset(0x00F6)]
    public readonly AgentModelType AgentModelType;

    [FieldOffset(0x0108)]
    public readonly TagInfo* Tags;

    [FieldOffset(0x010E)]
    public readonly byte Primary;

    [FieldOffset(0x010F)]
    public readonly byte Secondary;

    [FieldOffset(0x0110)]
    public readonly byte Level;

    [FieldOffset(0x0111)]
    public readonly byte TeamId;

    [FieldOffset(0x0118)]
    public readonly float EnergyRegen;

    [FieldOffset(0x0120)]
    public readonly float Energy;

    [FieldOffset(0x0124)]
    public readonly uint MaxEnergy;

    [FieldOffset(0x012C)]
    public readonly float HealthRegen;

    [FieldOffset(0x0134)]
    public readonly float Health;

    [FieldOffset(0x0138)]
    public readonly uint MaxHealth;

    [FieldOffset(0x013C)]
    public readonly uint Effects;

    [FieldOffset(0x0158)]
    public readonly LivingAgentModelState ModelState;

    [FieldOffset(0x015C)]
    public readonly LivingAgentState State;

    [FieldOffset(0x0184)]
    public readonly uint LoginNumber;

    [FieldOffset(0x01B5)]
    public readonly LivingAgentAllegiance Allegiance;

    public readonly bool IsAlive => (!this.State.HasFlag(LivingAgentState.Dead)) && this.Health > 0f;

    public readonly bool IsPlayer => this.LoginNumber is not 0;

    public readonly bool IsNpc => this.LoginNumber is 0;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 52)]
[GWCAEquivalent("MapAgent")]
public readonly struct MapAgentContext
{
    [FieldOffset(0x0000)]
    public readonly float CurrentEnergy;

    [FieldOffset(0x0004)]
    public readonly float MaxEnergy;

    [FieldOffset(0x0008)]
    public readonly float EnergyRegen;

    [FieldOffset(0x000C)]
    public readonly uint SkillTimestamp;

    [FieldOffset(0x0020)]
    public readonly float CurrentHealth;

    [FieldOffset(0x0024)]
    public readonly float MaxHealth;

    [FieldOffset(0x0028)]
    public readonly float HealthRegen;

    [FieldOffset(0x0030)]
    public readonly LivingAgentEffects Effects;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x38)]
[GWCAEquivalent("AgentInfo")]
public readonly unsafe struct AgentInfo
{
    [FieldOffset(0x0034)]
    public readonly char* NameEncoded;
}
