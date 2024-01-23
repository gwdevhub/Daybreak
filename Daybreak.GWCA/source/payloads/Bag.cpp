#pragma once
#include <cstdint>
#include <payloads/BagContent.h>
#include <payloads/Bag.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const Bag& p) {
        j = json
        {
            {"Items", p.Items},
        };
    }
}