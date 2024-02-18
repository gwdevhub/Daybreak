#include <cstdint>
#include <json.hpp>
#include <payloads/TitleInfoPayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const TitleInfoPayload& p) {
        j = json
        {
            {"TitleId", p.TitleId},
            {"TitleTierId", p.TitleTierId},
            {"TitleName", p.TitleName},
            {"CurrentPoints", p.CurrentPoints},
            {"PointsNeededNextRank", p.PointsNeededNextRank},
            {"IsPercentageBased", p.IsPercentageBased},
            {"CurrentTier", p.CurrentTier}
        };
    }
}