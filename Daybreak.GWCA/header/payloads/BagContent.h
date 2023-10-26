#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct BagContent {
        uint32_t Id = 0;
        uint32_t Slot = 0;
        uint32_t Count = 0;
        std::list<uint32_t> Modifiers;
    };

    void to_json(json& j, const BagContent& p);
}