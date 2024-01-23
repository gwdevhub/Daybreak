#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct PathingMetadataPayload {
        uint32_t TrapezoidCount;
    };

    void to_json(json& j, const PathingMetadataPayload& p);
}