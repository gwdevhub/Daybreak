namespace Daybreak.Shared.Models.Api;
public sealed record InstanceInfo(uint MapId, uint DistrictNumber, uint FoesKilled, uint FoesToKill, InstanceType Type, DistrictRegionInfo DistrictRegion, LanguageInfo Language, CampaignInfo Campaign, ContinentInfo Continent, RegionInfo Region, DifficultyInfo Difficulty)
{
}
