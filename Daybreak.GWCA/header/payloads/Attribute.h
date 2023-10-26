#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct Attribute {
        uint32_t Id;
        uint32_t BaseLevel;
        uint32_t ActualLevel;
        uint32_t DecrementPoints;
        uint32_t IncrementPoints;
    };

    void to_json(json& j, const Attribute& p);
}