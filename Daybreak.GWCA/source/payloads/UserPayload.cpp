#include <cstdint>
#include <json.hpp>
#include <payloads/UserPayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const UserPayload& p) {
        j = json
        {
            {"Email", p.Email},
            {"CurrentKurzickPoints", p.CurrentKurzickPoints},
            {"CurrentLuxonPoints", p.CurrentLuxonPoints},
            {"CurrentImperialPoints", p.CurrentImperialPoints},
            {"CurrentBalthazarPoints", p.CurrentBalthazarPoints},
            {"CurrentSkillPoints", p.CurrentSkillPoints},
            {"TotalKurzickPoints", p.TotalKurzickPoints},
            {"TotalLuxonPoints", p.TotalLuxonPoints},
            {"TotalImperialPoints", p.TotalImperialPoints},
            {"TotalBalthazarPoints", p.TotalBalthazarPoints},
            {"TotalSkillPoints", p.TotalSkillPoints},
            {"MaxKurzickPoints", p.MaxKurzickPoints},
            {"MaxLuxonPoints", p.MaxLuxonPoints},
            {"MaxImperialPoints", p.MaxImperialPoints},
            {"MaxBalthazarPoints", p.MaxBalthazarPoints},
        };
    }
}