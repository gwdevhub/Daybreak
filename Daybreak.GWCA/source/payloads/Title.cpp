#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/Title.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const Title& p) {
        j = json
        {
            {"Id", p.Id},
            {"CurrentPoints", p.CurrentPoints},
            {"PointsForCurrentRank", p.PointsForCurrentRank},
            {"PointsForNextRank", p.PointsForNextRank},
            {"TierNumber", p.TierNumber},
            {"MaxTierNumber", p.MaxTierNumber},
            {"IsPercentage", p.IsPercentage},
        };
    }
}