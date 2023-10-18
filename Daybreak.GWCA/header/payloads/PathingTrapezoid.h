#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct PathingTrapezoid {
        uint32_t Id;
        float XTL;
        float XTR;
        float XBL;
        float XBR;
        float YT;
        float YB;
    };

    void to_json(json& j, const PathingTrapezoid& p);
}