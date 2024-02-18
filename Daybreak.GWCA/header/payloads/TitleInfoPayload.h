#pragma once
#include <cstdint>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct TitleInfoPayload {
        uint32_t TitleId = 0;
        uint32_t TitleTierId = 0;
        uint32_t CurrentTier = 0;
        uint32_t CurrentPoints = 0;
        uint32_t PointsNeededNextRank = 0;
        bool IsPercentageBased = false;
        std::string TitleName = "";
    };

    void to_json(json& j, const TitleInfoPayload& p);
}