#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct Title {
        uint32_t Id = 0;
        uint32_t CurrentPoints = 0;
        uint32_t PointsForCurrentRank = 0;
        uint32_t PointsForNextRank = 0;
        uint32_t TierNumber = 0;
        uint32_t MaxTierNumber = 0;
        bool IsPercentage = false;
    };

    void to_json(json& j, const Title& p);
}