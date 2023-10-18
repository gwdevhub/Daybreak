#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct MapPayload {
        bool IsLoaded;
        uint32_t Id;
        uint32_t InstanceType;
        uint32_t Timer;
        uint32_t Campaign;
        uint32_t Continent;
        uint32_t Region;
        uint32_t Type;
    };

    void to_json(json& j, const MapPayload& p);
}