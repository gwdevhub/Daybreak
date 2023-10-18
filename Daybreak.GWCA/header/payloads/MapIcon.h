#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct MapIcon {
        uint32_t Id;
        float PosX;
        float PosY;
        uint32_t Affiliation;
    };

    void to_json(json& j, const MapIcon& p);
}