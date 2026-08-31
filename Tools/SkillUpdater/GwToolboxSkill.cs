using System.Text.Json.Serialization;

namespace Daybreak.Tools.SkillUpdater;

/// <summary>
/// One record of <c>https://api.gwtoolbox.com/v1/en/skills.json</c>, scanned
/// straight out of the Guild Wars client. Only the fields the generator needs
/// are modelled.
/// </summary>
/// <remarks>
/// The API omits any key equal to its default (<c>0</c>, <c>""</c>, <c>null</c>)
/// to keep the payload small, so every field here is nullable and a missing key
/// must be read as its zero value — see the "Omitted default-valued keys"
/// section of the API's README.
/// </remarks>
internal sealed class GwToolboxSkill
{
    [JsonPropertyName("id")] public int Id { get; init; }

    [JsonPropertyName("name")] public string? Name { get; init; }

    [JsonPropertyName("description")] public string? Description { get; init; }

    [JsonPropertyName("concise")] public string? Concise { get; init; }

    [JsonPropertyName("campaign")] public int Campaign { get; init; }

    [JsonPropertyName("profession")] public int Profession { get; init; }

    /// <summary>
    /// <c>GW::Constants::AttributeByte</c>, absent when the skill has no
    /// governing attribute. Id 0 (Fast Casting) is emitted explicitly, so a
    /// missing key is "none" rather than "Fast Casting".
    /// </summary>
    [JsonPropertyName("attribute")] public int? Attribute { get; init; }

    /// <summary><c>GW::Constants::SkillType</c> — a single value, not a bitmask.</summary>
    [JsonPropertyName("type")] public int Type { get; init; }

    /// <summary>
    /// Encoded cost rather than the literal one: values up to 10 are the cost
    /// itself, 11 means 15 and 12 means 25.
    /// </summary>
    [JsonPropertyName("energy_cost")] public int EnergyCost { get; init; }

    /// <summary>Adrenaline in the client's internal units; 25 per strike.</summary>
    [JsonPropertyName("adrenaline")] public int Adrenaline { get; init; }

    [JsonPropertyName("overcast")] public int Overcast { get; init; }

    /// <summary>Sacrificed health as a whole percentage (e.g. 17 for 17%).</summary>
    [JsonPropertyName("health_cost")] public int HealthCost { get; init; }

    [JsonPropertyName("activation")] public double Activation { get; init; }

    [JsonPropertyName("recharge")] public double Recharge { get; init; }

    /// <summary>
    /// Effect duration at 0 ranks, in game time units.
    /// <see cref="SkillMapper.MaintainedDuration"/> is the client's sentinel for
    /// a maintained enchantment, which is what Daybreak models as upkeep.
    /// </summary>
    [JsonPropertyName("duration0")] public int Duration0 { get; init; }

    /// <summary>Raw weapon-type requirement bitmask; see <see cref="SkillMapper"/>.</summary>
    [JsonPropertyName("weapon_req")] public int WeaponRequirement { get; init; }

    /// <summary>Dagger attack-chain position: 1 lead, 2 off-hand, 3 dual.</summary>
    [JsonPropertyName("combo")] public int Combo { get; init; }

    [JsonPropertyName("elite")] public int Elite { get; init; }

    [JsonPropertyName("touch_range")] public int TouchRange { get; init; }

    [JsonPropertyName("pve_only")] public int PvEOnly { get; init; }

    [JsonPropertyName("pvp_only")] public int PvPOnly { get; init; }
}
