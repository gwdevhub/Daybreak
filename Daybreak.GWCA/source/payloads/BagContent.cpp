#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <payloads/BagContent.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const BagContent& p) {
        j = json
        {
            {"Id", p.Id},
            {"Slot", p.Slot},
            {"Count", p.Count},
            {"Modifiers", p.Modifiers},
        };
    }
}